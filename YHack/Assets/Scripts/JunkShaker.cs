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
    private float pullSpeed = 150f;
    [SerializeField]
    private float pullRadius = 9f;
    [SerializeField]
    private Rigidbody2D rb;

    private PlayerController player;

    private Vector2 target;
    private float time = 0;
    private int right = 1;

    void Start() {
        target = Quaternion.AngleAxis(Random.value * 360 / maxRadius, Vector3.forward) * (Vector2.up * maxRadius);
        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

        CheckPushPull();
    }

    void CheckPushPull() {
        switch(player.PPState) {
            case PlayerController.PushPullState.None:
                break;
            case PlayerController.PushPullState.Pulling:
                Debug.Log("Getting Pulled");
                Vector2 thisToPlayer = (Vector2) player.transform.position - (Vector2) transform.position;
                if(thisToPlayer.magnitude <= pullRadius) {
                    rb.AddForce(thisToPlayer.normalized * pullSpeed);
                }
                break;
            case PlayerController.PushPullState.Pushing:
                Debug.Log("Getting Pulled");
                Vector2 playerToThis = (Vector2) transform.position - (Vector2) player.transform.position;
                if(playerToThis.magnitude <= pullRadius) {
                    rb.AddForce(playerToThis.normalized * pullSpeed);
                }
                break;
        }
    }

    
}
