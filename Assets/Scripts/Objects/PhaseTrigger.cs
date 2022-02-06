using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Events;

public class PhaseTrigger : MonoBehaviour
{
    public bool repeatable;
    public bool isInterctionOnly;
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
        if (enabled && !isInterctionOnly && col.CompareTag("Player"))
        {
            ChangePhase();
            if (!repeatable)
            {
                enabled = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isInterctionOnly && other.CompareTag("Player"))
        {
            bool isInteractable = other.GetComponent<PlayerController>().isInteracting;
            if (isInteractable)
            {
                isInteractable = false;
                ChangePhase();
            }
        }
    }
}
