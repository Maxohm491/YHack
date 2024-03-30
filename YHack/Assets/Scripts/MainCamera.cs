using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    public void LoadFirstLevel()
    {   
        SceneManager.LoadSceneAsync(1);
    }
}

