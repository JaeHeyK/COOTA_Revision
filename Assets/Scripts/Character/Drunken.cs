using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Drunken : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject hand;
    [SerializeField] private Animator animator;
    private int animatorVelocityHash;
    private int animatorMeleeHash;
    private int animatorThrowHash;
    private Rigidbody2D rb2D;
    private Collider2D cc2D;

    public float movementSpeed = 1.0f;
    public float maxSpeed = 1.0f;
    [SerializeField] private Vector3 leftScale;
    [SerializeField]private Vector3 rightScale;
    private bool isFlipped = true;
    [SerializeField]private int movementInput = 0;
    private bool throwInput;
    private bool meleeInput;
    public bool CanMove { get; set; }
    
    [SerializeField] public GameObject projectilePrefab;
    private GameObject projectileInstance;
    private Vector2 throwVector2;
    [SerializeField] public float throwForce = 1.0f;

    public UnityEvent OnAttack;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        animatorVelocityHash = Animator.StringToHash("Velocity");
        animatorMeleeHash = Animator.StringToHash("Melee");
        animatorThrowHash = Animator.StringToHash("Throw");
        
        rb2D = GetComponent<Rigidbody2D>();
        cc2D = GetComponent<Collider2D>();
    
        rightScale = transform.localScale;
        leftScale.Set(-rightScale.x, rightScale.y, rightScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMove) return;
        movementInput = Math.Sign((int)(target.transform.position.x - transform.position.x));
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("AttackTrigger"))
        {
            throwVector2 = col.GetComponent<throwData>().ThrowVector;
            throwInput = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            meleeInput = true;
        }
    }

    private void FixedUpdate()
    {
        UpdateDirection();

        UpdateVelocity();

        UpdateAttack();
    }

    private void UpdateDirection()
    {
        if (movementInput == -1 && !isFlipped)
        {
            isFlipped = true;
            transform.localScale = leftScale;
        } else if (movementInput == 1 && isFlipped)
        {
            isFlipped = false;
            transform.localScale = rightScale;
        }
    }

    private void UpdateVelocity()
    {
        Vector2 newVelocity = rb2D.velocity;

        newVelocity.x += movementInput * movementSpeed * Time.fixedDeltaTime;
        movementInput = 0;

        newVelocity.x = Mathf.Clamp(newVelocity.x, -maxSpeed, maxSpeed);

        rb2D.velocity = newVelocity;

        var speedNormalized = Math.Abs(newVelocity.x) / (maxSpeed*Time.fixedDeltaTime);
        animator.SetFloat(animatorVelocityHash, speedNormalized);
    }

    private void UpdateAttack()
    {
        if (meleeInput)
        {
            meleeInput = false;
            animator.SetTrigger(animatorMeleeHash);
        } else if (throwInput)
        {
            throwInput = false;
            animator.SetTrigger(animatorThrowHash);
        }
    }

   

    public void Melee()
    {
        Debug.Log("Melee Attack");
        OnAttack.Invoke();
    }

    public void ThrowProjectile()
    {
        projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.transform.position = hand.transform.position;
        projectileInstance.GetComponent<Rigidbody2D>().AddTorque(-20.0f);
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(throwVector2*throwForce, ForceMode2D.Impulse);
        Debug.Log("Throwing");
    }
}
