using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void MoveToStartScene()
    {
        Destroy(GameObject.Find("Player_puppet"));
        Destroy(GameObject.Find("SceneChangeManager"));
        Destroy(GameObject.Find("CameraManager"));
        Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene(0);
    }
}
