﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : SingletonNotDestroyed<SceneChangeManager>
{
    [SerializeField] private int currentSceneIndex;
    public int newSceneOrder = 0;
    
    protected SceneChangeManager() {}
    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialization();

        if (newSceneOrder == 1)
        {
            GameManager.Instance.PositionPlayer("StartSpawnPoint");
        } else if (newSceneOrder == -1)
        {
            GameManager.Instance.PositionPlayer("EndSpawnPoint");
        }
        newSceneOrder = 0;
        
        GameManager.Instance.OnSceneChanged();
        
        CameraManager.Instance.SetCamera();
        Debug.Log("current scene: " + SceneManager.GetSceneByBuildIndex(currentSceneIndex).name);
    }

    public IEnumerator LoadSceneByOffset(int offset)
    {
        AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(currentSceneIndex + offset);

        while (!asyncSceneLoad.isDone)
        {
            yield return null;
        }
    }

    public void OnPlayerLeft(int moveToScene)
    {
        Debug.Log("change to scene: "+moveToScene);
        Instance.newSceneOrder = moveToScene;
        StartCoroutine(Instance.LoadSceneByOffset(moveToScene));
    }
}