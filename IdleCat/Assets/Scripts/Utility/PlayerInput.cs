using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput;
    [SerializeField]
    private InputActionReference movement, Interact, Look;
    private void Update()
    {
        OnMovementInput?.Invoke(movement.action.ReadValue<Vector2>().normalized);
    }

    private void Start()
    {
        SaveManager.Instance.SaveCall += SavePlayerData;
        SaveManager.Instance.LoadCall += LoadPlayerData;
    }

    private void SavePlayerData()
    {

    }

    private void LoadPlayerData()
    {

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
            Vector2 WorldPoint = Camera.main.ScreenToWorldPoint(Look.action.ReadValue<Vector2>());
            RaycastHit2D raycastHit = Physics2D.Raycast(WorldPoint, Vector2.zero);

            Collider2D collider = raycastHit.collider;
            if (collider != null && collider.CompareTag("Intractable"))
            {
                Intractable[] hits =  raycastHit.collider.GetComponents<Intractable>();
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
