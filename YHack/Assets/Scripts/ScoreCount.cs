using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCount : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI score = GetComponent<TextMeshProUGUI>();
        string scoreString = PlayerPrefs.GetInt("asteroidsDestroyed", 0).ToString();
        score.text = $"Asteroids Destroyed: {scoreString}";
    }
}
