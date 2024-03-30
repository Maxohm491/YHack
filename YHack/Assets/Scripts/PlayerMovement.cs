using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject planet;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float gravStrength = 0.5f;

    [SerializeField]
    private float rocketPower = 1f;

    private float currUpSpeed = 0f;

    private State state;

    enum State {
        Grounded,
        Floating
    }

    // Update is called once per frame
    void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = State.Floating;
    }
    
    void FixedUpdate()
    {
        switch(state) {
            case State.Floating:
                FloatMovement();
                break;
        }
    }

    void FloatMovement() {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        Vector2 planetToShip = transform.position - planet.transform.position;
        Vector2 rotatedPos = Quaternion.AngleAxis(-hori * speed, Vector3.forward) * planetToShip;

        currUpSpeed -= gravStrength;
        currUpSpeed += rocketPower * vert;

        Vector2 upVector = rotatedPos.normalized * currUpSpeed;

        Vector2 newPos = rotatedPos + upVector;
        rb.MovePosition(newPos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.gameObject.CompareTag("Ground"))
        {
            state = State.Grounded;
            Debug.Log("grounded");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if (other.gameObject.CompareTag("Ground"))
        {
            state = State.Floating;
            Debug.Log("floating");
        }

    }
}
