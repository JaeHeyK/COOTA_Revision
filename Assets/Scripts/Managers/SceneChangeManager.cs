using System.Collections;
using Common;
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

        if (newSceneOrder == 1 || GameManager.Instance.CurrentPhase == GamePhase.init)
        {
            GameManager.Instance.PositionPlayer("StartSpawnPoint");
        } else if (newSceneOrder == -1)
        {
            GameManager.Instance.PositionPlayer("EndSpawnPoint");
        }
        else
        {
            Debug.Log("Repositioning");
            GameManager.Instance.RepositionPlayer();
        }
        newSceneOrder = 0;
        
        GameManager.Instance.OnSceneChanged();
        
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

    public void LoadNewScene(int moveToScene)
    {
        Instance.newSceneOrder = moveToScene;
        Debug.Log("Load Scene: " + SceneManager.GetSceneByBuildIndex(currentSceneIndex+moveToScene).name);
        StartCoroutine(Instance.LoadSceneByOffset(moveToScene));
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}