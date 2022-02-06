using System;
using System.Collections.Generic;
using Common;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : SingletonNotDestroyed<GameManager>
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private static PhaseTrigger[] phaseTriggers;
    [SerializeField] private static RespawnPoint[] respawnPoints;
    [SerializeField] private static RespawnPoint currentRespawnpoint;
    [SerializeField] private static GamePhase currentPhase = GamePhase.init;
    [SerializeField] private static bool isUpdatingPhase = false;
    [SerializeField] private static bool isRestartingPhase = false;
    
    private bool bPlayerInstantiated;
    
    public GamePhase CurrentPhase { get;  }
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
            Debug.Log("player not exists, instantiate one");

            bPlayerInstantiated = true;
            playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        }
    }

    public void PositionPlayer(string spawnPoint)
    {
        PlayerController.Instance.transform.position = GameObject.Find(spawnPoint).transform.position;
    }

    public void FindPhaseTriggers()
    {
        phaseTriggers = FindObjectsOfType<PhaseTrigger>();
    }

    public void FindRespawnPoints()
    {
        respawnPoints = FindObjectsOfType<RespawnPoint>();
    }

    public void onPhaseActivated()
    {
        isUpdatingPhase = true;
        UpdatePhase();
    }

    private void UpdatePhase()
    {
        currentPhase = (GamePhase)((int)currentPhase + 1) ;
        Debug.Log("Current phase is: " + currentPhase);
        
        EnablePhaseTrigger();
    }

    private void EnablePhaseTrigger()
    {
        foreach (var trigger in phaseTriggers)
        {
            if (trigger.Phase != (GamePhase)((int)currentPhase + 1))
            {
                trigger.enabled = false;
                continue;
            }
            trigger.enabled = true;
            isUpdatingPhase = false;
            isRestartingPhase = false;
            break;
        }
    }

    private void GetRespawnPoint()
    {
        foreach (var point in respawnPoints)
        {
            if (point.Phase == (GamePhase)((int)currentPhase + 1))
            {
                Debug.Log("current rp is : " + point);
                currentRespawnpoint = point;
                break;
            }
        }
    }

    public void OnSceneChanged()
    {
        FindPhaseTriggers();
        FindRespawnPoints();
        GetRespawnPoint();

        if (isUpdatingPhase || isRestartingPhase)
        {
            EnablePhaseTrigger();
        }
    }

    public void RestartPhase()
    {
        currentPhase = (GamePhase)((int)currentPhase - 1);
        Debug.Log("Phase after restart: " + currentPhase);
        isRestartingPhase = true;
    }
    
    
}