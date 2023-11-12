using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Vector2 movementVector;
    private Vector2 movement;
    private Animator animator;
    public Rigidbody2D rb2d;
    public float MaxSpeed;
    private bool isMoving;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rb2d = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    public void Move(Vector2 movementVector)
    {
        this.movementVector = movementVector;
        CalculateSpeed();
        movementVector *= MaxSpeed;
        movement = movementVector;
    }
    private void CalculateSpeed()
    {
        if (MathF.Abs(movementVector.y) == 0 && MathF.Abs(movementVector.x) == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
            CheckSide();
        }
    }

    private void CheckSide()
    {
        if (movementVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    private void FixedUpdate()
    {
        rb2d.velocity = movement * Time.deltaTime;
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }
}
