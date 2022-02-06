using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwData : MonoBehaviour
{

  
  [SerializeField] private Vector2 throwVector;
  public Vector2 ThrowVector => throwVector.normalized;
  

  private void Start()
  {

    
  }

  private void Update()
  {
  }
}
