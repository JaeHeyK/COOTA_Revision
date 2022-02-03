using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drunken : MonoBehaviour
{

    [SerializeField] public GameObject projectilePrefab;
    private GameObject projectileInstance;
    private Vector2 throwVector2;
    [SerializeField] public float throwForce = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("AttackTrigger"))
        {
            throwVector2 = col.GetComponent<throwData>().ThrowVector;
            ThrowProjectile();
        } else if (col.CompareTag("Player"))
        {
            Melee();
        }
    }

    private void Melee()
    {
        
    }

    private void ThrowProjectile()
    {
        projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.transform.position = gameObject.transform.position;
        projectileInstance.GetComponent<Rigidbody2D>().AddTorque(-20.0f);
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(throwVector2*throwForce, ForceMode2D.Impulse);
    }
}
