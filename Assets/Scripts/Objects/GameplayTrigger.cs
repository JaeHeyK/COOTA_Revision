using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Events;

public class GameplayTrigger : MonoBehaviour
{
    public bool repeatable;
    public UnityEvent onObjectActivated;

    private void Start()
    {
    }

    private void ChangePhase()
    {
        onObjectActivated.Invoke();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enabled && col.CompareTag("Player"))
        {
            ChangePhase();
            if (!repeatable)
            {
                enabled = false;
                Debug.Log("GameplayTrigger deactivated");
            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("Gameplay trigger of " + gameObject.name + " enabled");
    }
}
