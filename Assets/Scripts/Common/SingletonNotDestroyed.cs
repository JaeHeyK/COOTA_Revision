using System;
using UnityEngine;
using System.Collections;

public class SingletonNotDestroyed<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static object m_lock = new object();   // Using in lock()
    private static bool appIsClosing = false;       // To check that the game(include Unity Editor) is closing
    private static bool bExistingObject = false;

    public static T Instance
    {
        get 
        {
            if (appIsClosing)
            {   
                Debug.LogWarning("[Singleton] Instance"+ typeof(T) + " already destroyed. Returning null.");
                return null;
            }
            lock(m_lock)
            {
                if (instance == null)
                {
                    var foundObjects = FindObjectsOfType<T>();
        
                    if (foundObjects.Length > 0)
                    {
                        instance = foundObjects[0];
                    }
        
                    if (foundObjects.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }
        
                    if (instance == null)
                    {
                        string targetObjName = typeof(T).ToString();
                        GameObject targetObject = GameObject.Find(targetObjName);
                        if (targetObject == null)
                        {
                            targetObject = new GameObject(targetObjName);
                        }
        
                        instance = targetObject.AddComponent<T>();
                    }
                }
        
                return instance;
            }
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        Debug.Log("hello "+typeof(T));
    
        instance = this as T;
    }
    
    protected void OnApplicationQuit()
    {
        appIsClosing = true;
    }
    
    protected void OnDestroy()
    {
        appIsClosing = true;
    }
}
