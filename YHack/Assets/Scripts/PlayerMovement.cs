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
    float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        Vector2 planetToShip = transform.position - planet.transform.position;
        Vector2 newVector = Quaternion.AngleAxis(-hori * speed, Vector3.forward) * planetToShip;

        Vector2 grav = -planetToShip;

        rb.MovePosition(newVector);
    }
}
