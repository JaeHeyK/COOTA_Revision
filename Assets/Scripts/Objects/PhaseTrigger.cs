using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhaseTrigger : MonoBehaviour
{
    public bool repeatable;
    public UnityEvent onPhaseChanged;

    private void ChangePhase()
    {
        onPhaseChanged.Invoke();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enabled && col.CompareTag("Player"))
        {
            ChangePhase();
            if (!repeatable)
            {
                enabled = false;
                Debug.Log("PhaseTrigger deactivated");
            }
        }
    }
}
