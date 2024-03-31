using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField]
    private float maxFuel = 100f;

    // sprites
    [SerializeField]
    private Sprite baseSprite, leftSprite, rightSprite, noSprite;

    [SerializeField]
    private Slider slider;

    private SpriteRenderer spriteRenderer;

    private float fuel;

    private Vector2 velocity = new();

    private State state;
    enum State {
        Grounded,
        Floating
    }

    private int debrisDestroyed = 0;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = State.Floating;
        fuel = maxFuel;
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

        UpdateFuelDisplay();
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
        float vert = Math.Max(Input.GetAxisRaw("Vertical"), 0); // Remove back movement
        float hori = Input.GetAxisRaw("Horizontal");
        
        Vector2 planetToShip = transform.position - planet.transform.position;
        Vector2 grav = planetToShip.normalized * -gravStrength;

        if(fuel > 0) {
            velocity += rocketPower * vert * (Vector2) transform.up;
            

            transform.Rotate(0, 0, -rotateSpeed * hori, Space.Self);
        }

        if (vert == 0 || fuel <= 0) 
        {
            velocity = grav + velocity;
        }
 
        velocity = Vector2.ClampMagnitude(velocity, maxFloatSpeed);

        rb.MovePosition(velocity + (Vector2) transform.position);

        // Graphics and fuel updates
        if(vert != 0 && fuel > 0) {
            fuel = Math.Max(0, fuel-1);
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

        if(vert == 0 && hori !=0) {
            fuel -= 0.4f;
        }
    }
    
    void UpdateFuelDisplay() {
        slider.value = fuel/maxFuel * slider.maxValue;
    }

    void BecomeGrounded() 
    {
        Debug.Log("grounded");
        Vector2 planetToShip = transform.position - planet.transform.position;

        if(Vector2.Angle(planetToShip, transform.up) > 75) 
        {
            Debug.Log("crashed");
            SceneManager.LoadSceneAsync("GameOver");
        }
        velocity = new();

        spriteRenderer.sprite = noSprite;

        fuel = maxFuel;
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
            debrisDestroyed++;
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("debrisDestroyed", debrisDestroyed);
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
    } 
}
