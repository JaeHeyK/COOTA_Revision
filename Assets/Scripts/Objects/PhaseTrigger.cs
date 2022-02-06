using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Events;

public class PhaseTrigger : MonoBehaviour
{
    public bool repeatable;
    public GamePhase Phase = GamePhase.etc;
    public UnityEvent onPhaseActivated;

    private void Start()
    {
    }

    private void ChangePhase()
    {
        onPhaseActivated.Invoke();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enabled && col.CompareTag("Player"))
        {
            Debug.Log("Player triggering");
            ChangePhase();
            if (!repeatable)
            {
                enabled = false;
            }
        }
    }
}
