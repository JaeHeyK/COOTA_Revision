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
    [SerializeField] private static GamePhase currentPhase = GamePhase.init;
    [SerializeField] private static bool isUpdatingPhase = false;
    
    private bool bPlayerInstantiated;
    protected GameManager() {}

    private void Start()
    {
        Initialization();
        Debug.Log("Started");
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

    public void onPhaseActivated()
    {
        Debug.Log("phase activated!");
        isUpdatingPhase = true;
        UpdatePhase();
    }

    private void UpdatePhase()
    {
        currentPhase = (GamePhase)((int)currentPhase + 1) ;
        Debug.Log("current phase is " + currentPhase);
        Debug.Log("next phase is " + (GamePhase)((int)currentPhase + 1));
        EnablePhaseTrigger();
        Debug.Log("is updating?: " + isUpdatingPhase);

    }

    private void EnablePhaseTrigger()
    {
        Debug.Log("searching for next phase...");

        foreach (var trigger in phaseTriggers)
        {
            Debug.Log("Comparing trigger: " + trigger.Phase + " with next: " + (GamePhase)((int)currentPhase + 1));
            if (trigger.Phase != (GamePhase)((int)currentPhase + 1)) continue;
            Debug.Log("Activating trigger: " + trigger.Phase);
            trigger.enabled = true;
            isUpdatingPhase = false;
            break;
        }
        Debug.Log("next phase updated: " + !isUpdatingPhase);

    }

    public void OnSceneChanged()
    {
        FindPhaseTriggers();
        Debug.Log("still updating?: " + isUpdatingPhase);
        Debug.Log("After scene changed, current phase is " + currentPhase);

        if (isUpdatingPhase)
        {
            Debug.Log("scene changed, and update phase!");
            EnablePhaseTrigger();
        }
    }
    
}