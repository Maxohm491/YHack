using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkMover : MonoBehaviour
{
    [SerializeField]
    private Transform junkPoint;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float moveSpeed = 0.01f;
    [SerializeField]
    private float maxRadius = 3f;

    private Vector2 target;
    private float time = 0;
    private int right = 1;

    void Start() {
        target = Quaternion.AngleAxis(Random.value * 360 / maxRadius, Vector3.forward) * (Vector2.up * maxRadius);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(time <= 0) {
            right *= -1;
            time = Random.value*350; 
        }
        else {
            time --;
        }

        target = Quaternion.AngleAxis(speed * right / maxRadius, Vector3.forward) * target;

        Vector2 direction = target + (Vector2) junkPoint.position - (Vector2) transform.position;

        transform.position = (Vector2) transform.position + direction.normalized * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit by player");
            Destroy(transform.parent.gameObject);
        }
    }
}
