using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : SingletonNotDestroyed<GameManager>
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private PlayerController playerController = null;
    
    private bool bPlayerInstantiated;
    protected GameManager() {}

    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        bPlayerInstantiated = false;
        
        CheckPlayerPresence();

        if (bPlayerInstantiated)
        {
            CameraManager.Instance.SetCamera();
        }
    }

    private void CheckPlayerPresence()
    {
        playerController = PlayerController.Instance;

        if (playerController is null)
        {
            bPlayerInstantiated = true;
            playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        }
    }

    public void PositionPlayer(string spawnPoint)
    {
        PlayerController.Instance.transform.position = GameObject.Find(spawnPoint).transform.position;
    }

    public void onPhaseChanged()
    {
        Debug.Log("phase changed!");
    }
    
}