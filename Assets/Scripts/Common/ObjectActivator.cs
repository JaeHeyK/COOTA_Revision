using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] string activatorTag = null;
    [SerializeField] bool deactivateOnExit = false;
    [SerializeField] GameObject[] objects = null;
    
    void Start() {}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(activatorTag))
        {
            foreach (var obj in objects)
                obj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (deactivateOnExit && collision.CompareTag(activatorTag))
        {
            foreach (var obj in objects)
                obj.SetActive(false);
        }
    }
}
