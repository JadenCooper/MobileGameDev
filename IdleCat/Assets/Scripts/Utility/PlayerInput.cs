using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput, OnBuildMovementInput, onPress;
    [SerializeField]
    private InputActionReference movement, Interact, Look;

    public bool BuildMode;

    private Vector2 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;
    private void Update()
    {
        if (BuildMode)
        {
            OnBuildMovementInput?.Invoke(movement.action.ReadValue<Vector2>().normalized);
        }
        else
        {
            OnMovementInput?.Invoke(movement.action.ReadValue<Vector2>().normalized);
        }
    }

    private void Start()
    {
        SaveManager.Instance.SaveCall += SavePlayerData;
        SaveManager.Instance.LoadCall += LoadPlayerData;
    }

    private void SavePlayerData()
    {
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.Position = transform.position;
        saveData.Flipped = GetComponent<SpriteRenderer>().flipX;
        SaveData.current.PSD = saveData;
    }

    private void LoadPlayerData()
    {
        transform.position = SaveData.current.PSD.Position;
        GetComponent<SpriteRenderer>().flipX = SaveData.current.PSD.Flipped;
    }

    private void OnEnable()
    {
        Interact.action.performed += Press;
    }
    private void OnDisable()
    {
        Interact.action.performed -= Press;
    }

    private void Press(InputAction.CallbackContext obj)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (BuildMode)
            {
                Vector2 pressPos = Look.action.ReadValue<Vector2>();
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pressPos), Vector2.zero, 100, placementLayerMask);
                if (hit.collider != null)
                {
                    Debug.Log("IN");
                    lastPosition = hit.point;
                    onPress?.Invoke(lastPosition);
                }
                //return lastPosition;
            }
            else
            {
                Vector2 WorldPoint = Camera.main.ScreenToWorldPoint(Look.action.ReadValue<Vector2>());
                RaycastHit2D raycastHit = Physics2D.Raycast(WorldPoint, Vector2.zero);

                Collider2D collider = raycastHit.collider;
                if (collider != null && collider.CompareTag("Intractable"))
                {
                    Intractable[] hits = raycastHit.collider.GetComponents<Intractable>();
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].enabled == true)
                        {
                            hits[i].Interact(gameObject.transform); // Activates Object's Interaction
                        }
                    }
                }
            }
        }
    }
}
