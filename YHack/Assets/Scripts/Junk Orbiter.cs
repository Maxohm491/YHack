using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkOrbiter : MonoBehaviour
{
    private Vector2 center;
    private float dist;

    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private Rigidbody2D rb;
    
    void Start() {
        center = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        speed += Random.Range(-speed/4, speed/4);
        dist = (center - (Vector2) transform.position).magnitude; 
    }

    void FixedUpdate()
    {
        Vector2 rotatedPos = Quaternion.AngleAxis(speed / dist, Vector3.forward) * transform.position;
        rb.MovePosition(rotatedPos);
    }
}
