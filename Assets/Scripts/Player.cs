using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground")] [SerializeField] private Transform groundCheck = null;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask whatIsGround = new LayerMask();

    private InputManager inputManager;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private float moveInput = 0;
    private bool wantsToJump = false;
    private bool isGrounded = true;
    private bool facingRight = true;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        inputManager.OnHorizontalAxis += WantsToMoveHorizontal;
        inputManager.OnHorizontalAxis += CheckFlip;
        inputManager.OnHorizontalAxisZero += StopRunning;
        inputManager.OnJump += WantsToJump;
    }

    private void FixedUpdate()
    {
        bool needsLanding = !isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (needsLanding && isGrounded)
        {
            animator.SetTrigger("lands");
        }

        if (moveInput != 0)
        {
            MoveHorizontal(moveInput);
        }
        else
        {
            StopMovingHorizontal();
        }

        if (wantsToJump)
        {
            Jump();
        }
    }

    private void WantsToMoveHorizontal(float input)
    {
        moveInput = input;
    }

    private void MoveHorizontal(float input)
    {
        if (isGrounded)
        {
            animator.SetBool("isRunning", true);
        }

        rigidBody.velocity = new Vector2(input * moveSpeed, rigidBody.velocity.y);
    }

    private void StopMovingHorizontal()
    {
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }

    private void CheckFlip(float moveInput)
    {
        if ((facingRight && moveInput < 0) || (!facingRight && moveInput > 0))
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        if (facingRight)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }

    private void WantsToJump()
    {
        wantsToJump = true;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            animator.SetTrigger("jumps");
            rigidBody.velocity = Vector2.up * jumpForce;
            wantsToJump = false;
        }
    }

    private void StopRunning()
    {
        moveInput = 0;
        animator.SetBool("isRunning", false);
    }
}