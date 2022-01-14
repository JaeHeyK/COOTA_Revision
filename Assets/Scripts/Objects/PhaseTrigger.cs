using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhaseTrigger : MonoBehaviour
{
    public UnityEvent onPhaseChanged;
    // Start is called before the first frame update

    private void ChangePhase()
    {
        onPhaseChanged.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        ChangePhase();
    }
}
