using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : SingletonNotDestroyed<SceneChangeManager>
{
    [SerializeField] private int currentSceneIndex;
    
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
        
        GameManager.Instance.PositionPlayer();
        CameraManager.Instance.SetCamera();
    }

    public IEnumerator LoadSceneByOffset(int offset)
    {
        AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(currentSceneIndex + offset);

        while (!asyncSceneLoad.isDone)
        {
            yield return null;
        }
    }
}