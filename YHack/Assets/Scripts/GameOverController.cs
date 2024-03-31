using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void LoadPreviousLevel()
    {
        SceneManager.LoadSceneAsync(PlayerPrefs.GetString("sceneName"));
    }
}