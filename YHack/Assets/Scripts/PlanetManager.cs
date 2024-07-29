using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public float radius;

    // Update is called once per frame
    void Awake()
    {
        radius = transform.localScale.x / 2;
    }
}
