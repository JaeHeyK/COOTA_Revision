using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : SingletonNotDestroyed<PlayerController>
{ 
    public Vector2 movementSpeed = new Vector2(50f, 1f);
    public float jumpSpeed = 20.0f;
    private Vector2 movementInput;
    private Rigidbody2D rb2D;
    private Collider2D cc2D;
    private LayerMask groundMask;
    private LayerMask prevDoorMask;
    private LayerMask nextDoorMask;

    public bool CanMove { get; set; }
    public bool CamClimb { get; set; }
    private bool isClimbing;
    private bool isJumping;
    private bool isFalling;
    private bool jumpInput;
    private bool interactionInput;
    public bool isInteracting;
    private bool isLeaving;
    public bool isFlipped = true;
    private int forcedMoveInput = 0;

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

    public UnityEvent OnPlayerDead;
    public UnityEvent OnInteract;
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
        CamClimb = false;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        float moveHorizontal = 0.0f;
        if ((CanMove && keyboard.leftArrowKey.isPressed) || forcedMoveInput == -1)
        {
            moveHorizontal = -1.0f;
        } else if ((CanMove && keyboard.rightArrowKey.isPressed) || forcedMoveInput == 1)
        {
            moveHorizontal = 1.0f;
        }

        float moveVertical = 0.0f;
        if (CamClimb)
        {
            if (keyboard.upArrowKey.isPressed)
            {
                moveVertical = 1.0f;
                isClimbing = true;
            } else if (keyboard.downArrowKey.isPressed)
            {
                moveVertical = -1.0f;
                isClimbing = true;
            }

            if (cc2D.IsTouchingLayers(groundMask))
            {
                isClimbing = false;
            }
        }
        else
        {
            isClimbing = false;
        }
        
        
        movementInput = new Vector2(moveHorizontal, moveVertical);

        if (CanMove && !isJumping && keyboard.spaceKey.wasPressedThisFrame)
        {
            jumpInput = true;
        }

        if (CanMove && !interactionInput && keyboard.ctrlKey.wasPressedThisFrame)
        {
            interactionInput = true;
        }
    }

    private void FixedUpdate()
    {
        UpdateDirection();
        
        UpdateVelocity();
        
        UpdateJump();

        UpdateGravityScale();

        UpdateInteraction();
    }
    
    private void UpdateVelocity()
    {
        Vector2 velocity = rb2D.velocity;
        if (!isClimbing)
        {
            velocity += movementInput * movementSpeed * Time.fixedDeltaTime;
        } else 
        {
            velocity.x = 0;
            if (movementInput.y != 0)
            {
                velocity.y = movementInput.y * movementSpeed.y * Time.fixedDeltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
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

        if (isClimbing)
        {
            gravityScale = 0.0f;
        } else if (!cc2D.IsTouchingLayers(groundMask))
        {
            gravityScale = rb2D.velocity.y < 0 ? fallGravityScale : jumpGravityScale;
        }

        rb2D.gravityScale = gravityScale;
    }

    private void UpdateInteraction()
    {
        switch (interactionInput)
        {
            case true when cc2D.IsTouchingLayers(prevDoorMask):
                onPlayerLeft.Invoke(-1);
                break;
            case true when cc2D.IsTouchingLayers(nextDoorMask):
                onPlayerLeft.Invoke(1);
                break;
            case true when cc2D.IsTouchingLayers(LayerMask.GetMask("Interactable")):
                isInteracting = true;
                break;
        }

        interactionInput = false;
    }

    public void ChangeDirection(int direction)
    {
        if (direction == -1)
        {
            isFlipped = false;
            transform.localScale = originalScale;
        }
        else if(direction == 1)
        {
            isFlipped = true;
            transform.localScale = flippedScale;
        }
    }

    public void ForceMovement(int direction)
    {
        forcedMoveInput = direction;
    }

    public void Die()
    {
        CanMove = false;
        Debug.Log("player dead");
        OnPlayerDead.Invoke();
    }

}
