using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Events;

public class RespawnPoint : MonoBehaviour
{
    public GamePhase Phase = GamePhase.etc;

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    private void Start()
    {
    }

}
