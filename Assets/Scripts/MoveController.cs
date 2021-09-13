using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    public float jumpSpeed = 20.0f;
    private Vector2 movementInput;
    private Rigidbody2D rb2D;
    private Collider2D cc2D;
    private LayerMask groundMask;

    private bool isJumping;
    private bool isFalling;
    private bool jumpInput;

    [SerializeField] private float maxSpeed = 1.0f;
    
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cc2D = GetComponent<Collider2D>();
        groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        float moveHorizontal = 0.0f;
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            moveHorizontal = -1.0f;
        } else if (Keyboard.current.rightArrowKey.isPressed)
        {
            moveHorizontal = 1.0f;
        }

        movementInput = new Vector2(moveHorizontal, 0);
        
        
        if (!isJumping && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpInput = true;
        }
    }

    private void FixedUpdate()
    {

        UpdateVelocity();
        if (isJumping && rb2D.velocity.y < 0)
        {
            isFalling = true;
        }
        

        
        if (jumpInput && cc2D.IsTouchingLayers(groundMask)) {
            rb2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpInput = false;
            isJumping = true;
        } else if (isJumping && isFalling && cc2D.IsTouchingLayers(groundMask) ) {
            isJumping = false;
            isFalling = false;
        }
        
    }

    private void UpdateVelocity()
    {
        Vector2 velocity = rb2D.velocity;
        velocity += movementInput * (movementSpeed * Mathf.Abs(movementInput.x)) * Time.fixedDeltaTime;
        
        movementInput = Vector2.zero;

        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        
        rb2D.velocity = velocity;
        Debug.Log("x velocity: " + velocity.x);
    }
}
