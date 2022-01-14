using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private enum SceneOffset
    {
        Previous = -1,
        Next = 1
    };

    [SerializeField] private SceneOffset moveToScene = SceneOffset.Next;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(SceneChangeManager.Instance.LoadSceneByOffset((int)moveToScene));
    }
    
    private void OnDrawGizmos()
    {
        var collider = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)collider.offset, collider.size);
    }
}
