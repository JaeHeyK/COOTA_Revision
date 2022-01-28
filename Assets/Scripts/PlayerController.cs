using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : SingletonNotDestroyed<PlayerController>
{ 
    public float movementSpeed = 3.0f;
    public float jumpSpeed = 20.0f;
    private Vector2 movementInput;
    private Rigidbody2D rb2D;
    private Collider2D cc2D;
    private LayerMask groundMask;
    private LayerMask prevDoorMask;
    private LayerMask nextDoorMask;

    public bool CanMove { get; set; }
    private bool isJumping;
    private bool isFalling;
    private bool jumpInput;
    private bool interactionInput;
    private bool isLeaving;
    private bool isFlipped = true;

    private readonly Vector3 originalScale = new Vector3(-1f, 1f, 1f);
    readonly Vector3 flippedScale = new Vector3(1f, 1f, 1f);

    [SerializeField] private Animator animator = null;
    [SerializeField] private float maxSpeed = 1.0f;
    [SerializeField] private float defaultGravityScale = 1.0f;
    [SerializeField] private float jumpGravityScale = 1.0f;
    [SerializeField] private float fallGravityScale = 2.0f;
    [SerializeField] private float flipThreshold = 0.1f;

    private int animatorRunningSpeed;
    
    [Serializable]
    public class InteractionEvent : UnityEvent<int> {}
    public InteractionEvent onPlayerLeft;
    protected PlayerController() {}

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cc2D = GetComponent<Collider2D>();
        groundMask = LayerMask.GetMask("Ground");
        prevDoorMask = LayerMask.GetMask("PrevDoor");
        nextDoorMask = LayerMask.GetMask("NextDoor");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");

        CanMove = true;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (!CanMove || keyboard == null) return;
        float moveHorizontal = 0.0f;
        if (keyboard.leftArrowKey.isPressed)
        {
            moveHorizontal = -1.0f;
        } else if (keyboard.rightArrowKey.isPressed)
        {
            moveHorizontal = 1.0f;
        }
        
        
        movementInput = new Vector2(moveHorizontal, 0);

        if (!isJumping && keyboard.spaceKey.wasPressedThisFrame)
        {
            jumpInput = true;
        }

        if (!interactionInput && keyboard.ctrlKey.wasPressedThisFrame)
        {
            interactionInput = true;
        }
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
        
        UpdateJump();
        
        UpdateDirection();
        
        UpdateGravityScale();

        UpdateInteraction();
    }
    
    private void UpdateVelocity()
    {
        Vector2 velocity = rb2D.velocity;
        velocity += movementInput * movementSpeed * Time.fixedDeltaTime;
        movementInput = Vector2.zero;
        
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        
        rb2D.velocity = velocity;

        var horizontalSpeedNormalized = Math.Abs(velocity.x) / maxSpeed;
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
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

    private void UpdateDirection()
    {
        if (rb2D.velocity.x > flipThreshold && isFlipped)
        {
            isFlipped = false;
            transform.localScale = originalScale;
        } else if (rb2D.velocity.x < -flipThreshold && !isFlipped)
        {
            isFlipped = true;
            transform.localScale = flippedScale;
        }
    }

    private void UpdateGravityScale()
    {
        float gravityScale = defaultGravityScale;

        if (!cc2D.IsTouchingLayers(groundMask))
        {
            gravityScale = rb2D.velocity.y < 0 ? fallGravityScale : jumpGravityScale;
        }

        rb2D.gravityScale = gravityScale;
    }

    private void UpdateInteraction()
    {
        if(interactionInput && cc2D.IsTouchingLayers(prevDoorMask))
        {
            onPlayerLeft.Invoke(-1);
        }
        
        if(interactionInput && cc2D.IsTouchingLayers(nextDoorMask))
        {
            onPlayerLeft.Invoke(1);
        }

        interactionInput = false;
    }

}
