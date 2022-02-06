using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private LayerMask groundMask;
    private LayerMask playerMask;

    public UnityEvent OnAttackPlayer;

    public bool isLethal = true;
    // Start is called before the first frame update
    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isLethal)
        {
            if (col.gameObject.layer == groundMask)
            {
                isLethal = false;
                GetComponent<Collider2D>().isTrigger = true;
            } else if (col.gameObject.CompareTag("Player"))
            {
                OnAttackPlayer.Invoke();
            }
        }
    }
}
