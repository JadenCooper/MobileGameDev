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
    public float currentSpeed;
    public float Acceleration;
    public float MaxSpeed;
    private bool isMoving;
    private SpriteRenderer spriteRenderer;
    public bool isJumping = false;

    public float jumpPower = 15;
    public bool isGrounded = true;
    public Transform groundCheck;
    public LayerMask groundLayer;
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
            currentSpeed += Acceleration * Time.deltaTime;
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
        if (!isJumping)
        {
            if (movement.y >= 0.5)
            {
                isJumping = true;
                movement.y = jumpPower;
                rb2d.velocity = movement * Time.deltaTime;
            }
            else
            {
                movement.y = 0;
                animator.SetBool("isMoving", isMoving);
            }
            rb2d.velocity = movement * Time.deltaTime;
        }
        else
        {
            isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.24f, 0.02f), CapsuleDirection2D.Horizontal, 0, groundLayer);
            Debug.Log("Check");
            if (isGrounded)
            {
                Debug.Log("Grounded");
                isJumping = false;
            }
        }
    }
}
