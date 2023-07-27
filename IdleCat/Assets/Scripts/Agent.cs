using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Agent : MonoBehaviour
{
    public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();
    private Vector2 movementInput;
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    void Update()
    {
        GetBodyMovement();
    }
    private void GetBodyMovement()
    {
        OnMoveBody?.Invoke(MovementInput);
    }
}
