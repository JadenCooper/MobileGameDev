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
                raycastHit.collider.GetComponent<Intractable>().Interact(gameObject.transform); // Activates Object's Interaction
            }
        }
    }
}
