using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI score = GetComponent<TextMeshProUGUI>();
        string timeString = PlayerPrefs.GetString("time");
        score.text = $"Time: {timeString}";
    }
}
