using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkShaker : MonoBehaviour
{
    [SerializeField]
    private Transform junkPoint;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float moveSpeed = 0.01f;
    [SerializeField]
    private float maxRadius = 3f;
    [SerializeField]
    private Rigidbody2D rb;

    private Vector2 target;
    private float time = 0;
    private int right = 1;

    void Start() {
        target = Quaternion.AngleAxis(Random.value * 360 / maxRadius, Vector3.forward) * (Vector2.up * maxRadius);
        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
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

        rb.MovePosition((Vector2) transform.position + direction.normalized * moveSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
