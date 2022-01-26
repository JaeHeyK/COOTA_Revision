using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public enum SceneOffset
    {
        Previous = -1,
        Next = 1
    };

    [SerializeField] public SceneOffset moveToScene = SceneOffset.Next;

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (!other.CompareTag("Player")) return;
    //     SceneChangeManager.Instance.newSceneOrder = (int)moveToScene;
    //     StartCoroutine(SceneChangeManager.Instance.LoadSceneByOffset((int)moveToScene));
    // }
    
    private void OnDrawGizmos()
    {
        var collider = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)collider.offset, collider.size);
    }
}
