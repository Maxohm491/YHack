using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadFirstLevel()
    {   
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void LoadInfinite()
    {   
        Debug.Log("Loading infinite");
        SceneManager.LoadSceneAsync("Infinite Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application Exited");
    }
}
