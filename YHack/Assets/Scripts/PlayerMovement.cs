using System;
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
    private float groundSpeed = 6f;
    [SerializeField]
    private float floatSpeed = 1f;
    [SerializeField]
    private float gravStrength = 0.5f;
    [SerializeField]
    private float rocketPower = 1f;
    [SerializeField]
    private float maxFloatSpeed = 10f;

    private float currUpSpeed = 0f;
    private float currHoriSpeed = 0f;


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
                FloatingMovement();
                break;
            case State.Grounded:
                GroundedMovement();
                break;
        }
    }

    void GroundedMovement() {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        Vector2 planetToShip = transform.position - planet.transform.position;
        Vector2 rotatedPos = Quaternion.AngleAxis(-hori * groundSpeed / planetToShip.magnitude, Vector3.forward) * planetToShip;

        if(vert == 1) {
            // lil jump
            currUpSpeed += rocketPower * 4;
            currHoriSpeed = -hori * groundSpeed;

            Vector2 upVector = rotatedPos.normalized * currUpSpeed;
            rotatedPos += upVector;
        }

        rb.MovePosition(rotatedPos);
        transform.up = rotatedPos;
    }

    void FloatingMovement() {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        currHoriSpeed += -hori * floatSpeed;
        currHoriSpeed = Math.Clamp(currHoriSpeed, -maxFloatSpeed, maxFloatSpeed);

        Vector2 planetToShip = transform.position - planet.transform.position;
        Vector2 rotatedPos = Quaternion.AngleAxis(currHoriSpeed / planetToShip.magnitude, Vector3.forward) * planetToShip;

        currUpSpeed -= gravStrength;
        currUpSpeed += rocketPower * vert;

        Vector2 upVector = rotatedPos.normalized * currUpSpeed;

        Vector2 newPos = rotatedPos + upVector;
        rb.MovePosition(newPos);
        transform.up = rotatedPos;
    }

    void BecomeGrounded() {
        Debug.Log("grounded");
        currUpSpeed = 0f;
        currHoriSpeed = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.gameObject.CompareTag("Ground"))
        {
            state = State.Grounded;
            BecomeGrounded();
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
