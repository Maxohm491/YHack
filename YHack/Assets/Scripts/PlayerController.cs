using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject planet;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float groundSpeed = 6f;
    [SerializeField]
    private float gravStrength = 0.5f;
    [SerializeField]
    private float rocketPower = 1f;
    [SerializeField]
    private float maxFloatSpeed = 10f;
    [SerializeField]
    private float rotateSpeed = 3.5f;

    // sprites
    [SerializeField]
    private Sprite baseSprite;
    [SerializeField]
    private Sprite leftSprite;
    [SerializeField]
    private Sprite rightSprite;
    [SerializeField]
    private Sprite noSprite;

    private SpriteRenderer spriteRenderer;


    private Vector2 velocity = new();

    private State state;
    enum State {
        Grounded,
        Floating
    }

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = State.Floating;
    }
    
    void FixedUpdate()
    {
        switch(state) 
        {
            case State.Floating:
                FloatingMovement();
                break;
            case State.Grounded:
                GroundedMovement();
                break;
        }
    }

    void GroundedMovement() 
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        Vector2 planetToShip = transform.position - planet.transform.position;
        Vector2 rotatedPos = Quaternion.AngleAxis(-hori * groundSpeed / planetToShip.magnitude, Vector3.forward) * planetToShip;

        if(vert == 1) 
        {
            velocity = planetToShip.normalized * 17 * rocketPower;
            rotatedPos += velocity;
        }

        rb.MovePosition(rotatedPos);
        transform.up = rotatedPos;
    }

    void FloatingMovement() 
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");
        
        Vector2 planetToShip = transform.position - planet.transform.position;
        Vector2 grav = planetToShip.normalized * -gravStrength;

        velocity += rocketPower * vert * (Vector2) transform.up;
        if (vert == 0) 
        {
            velocity = grav + velocity;
        }
        velocity = Vector2.ClampMagnitude(velocity, maxFloatSpeed);

        rb.MovePosition(velocity + (Vector2) transform.position);
        transform.Rotate(0, 0, -rotateSpeed * hori, Space.Self);

        if(vert != 0) {
            if(hori == 0) {
                spriteRenderer.sprite = baseSprite;
            }
            else if (hori == 1) {
                spriteRenderer.sprite = leftSprite;
            }
            else {
                spriteRenderer.sprite = rightSprite;
            }
        }
        else {
            spriteRenderer.sprite = noSprite;
        }
    }

    void BecomeGrounded() 
    {
        Debug.Log("grounded");
        velocity = new();

        spriteRenderer.sprite = noSprite;

        Vector2 planetToShip = transform.position - planet.transform.position;
        if(Vector2.Angle(planetToShip, transform.up) > 75) 
        {
            Debug.Log("crashed");
            SceneManager.LoadSceneAsync("GameOver");
        }
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Junk"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
    } 
}
