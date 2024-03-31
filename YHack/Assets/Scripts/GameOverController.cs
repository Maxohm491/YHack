using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    AudioController audioController;

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void Start() {
        audioController = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioController>();
        audioController.StopAll();
    }

    public void LoadPreviousLevel()
    {
        SceneManager.LoadSceneAsync(PlayerPrefs.GetString("sceneName"));
    }
}