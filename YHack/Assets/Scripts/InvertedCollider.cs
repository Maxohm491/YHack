using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitInverted2DEdges : MonoBehaviour {
    
    [SerializeField]
    private int NumEdges;
    [SerializeField]
    private float Radius;

    void Start () 
    {
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] points = new Vector2[NumEdges + 1];

        for (int i = 0; i < NumEdges; i++)
        {
            float angle = 2 * Mathf.PI * i / NumEdges;
            float x = Radius * Mathf.Cos(angle);
            float y = Radius * Mathf.Sin(angle);

            points[i] *= new Vector2(x, y);
        }

        points[NumEdges] = points[0];
        edgeCollider.points = points;
    }

}