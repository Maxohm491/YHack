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
    
    [SerializeField]
    private float pullSpeed = 150f;
    [SerializeField]
    private float pullRadius = 9f;
    private PlayerController player;

    void Start() {
        center = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        speed += Random.Range(-speed/4, speed/4);
        dist = (center - (Vector2) transform.position).magnitude; 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        Vector2 rotatedPos = Quaternion.AngleAxis(speed / dist, Vector3.forward) * transform.position;
        rb.MovePosition(rotatedPos);
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
