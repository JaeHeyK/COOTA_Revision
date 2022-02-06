using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimEvent : MonoBehaviour
{
    public UnityEvent OnThrowAnim;

    public UnityEvent OnMeleeAnim;
    // Start is called before the first frame update
    public void OnThrow()
    {
        OnThrowAnim.Invoke();
    }

    public void OnMelee()
    {
        OnMeleeAnim.Invoke();
    }
}
