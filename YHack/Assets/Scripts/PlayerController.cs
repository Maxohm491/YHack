using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
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
    private float maxFuel = 300f;
    [SerializeField]
    private float planetRadius = 12.5f;
    private float bottomOfShip = 0.865f;

    // sprites
    [SerializeField]
    private Sprite baseSprite, leftSprite, rightSprite, noSprite, leftSmallSprite, rightSmallSprite;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Collider2D shipColl;
    [SerializeField]
    private Collider2D forceFieldColl;

    private SpriteRenderer spriteRenderer;

    private GameObject forceField;

    private float fuel;

    private Vector2 velocity = new();

    private State state;
    enum State {
        Grounded,
        Floating
    }

    private PushPullState ppState;
    public PushPullState PPState { get{ return ppState; } }
    public enum PushPullState {
        None,
        Pushing,
        Pulling
    }

    private int debrisDestroyed = 0;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = State.Floating;
        forceField = transform.GetChild(0).gameObject;
        fuel = maxFuel;
        ppState = PushPullState.None;
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
                fuel = maxFuel;
                break;
        }

        UpdateFuelDisplay();
        HandleForceField();
        HandlePushPull();
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

    void HandleForceField() {
        if(Input.GetKey(KeyCode.LeftShift) && fuel > 0) {
            forceField.SetActive(true);
            fuel -= 0.5f;
        }
        else {
            forceField.SetActive(false);
        }
    }

    void HandlePushPull() {
        if(Input.GetKey(KeyCode.Z) && fuel > 0) {
            ppState = PushPullState.Pulling;
            fuel -= 0.25f;
        }
        else if (Input.GetKey(KeyCode.X) && fuel > 0) {
            ppState = PushPullState.Pushing;
            fuel -= 0.25f;
        }
        else {
            ppState = PushPullState.None;
        }
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

        if(vert == 0 && hori != 0) 
        {
            if (hori > 0)
            {
                spriteRenderer.sprite = leftSmallSprite; 
            }
            else if(hori < 0)
            {
                spriteRenderer.sprite = rightSmallSprite;
            }
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
            Crashed();
        }
        velocity = new();
        transform.up = planetToShip;
        Vector2 shadow = planetToShip.normalized * (planetRadius + (bottomOfShip/2));
        transform.position = new Vector3(shadow.x, shadow.y, -1);

        spriteRenderer.sprite = noSprite;

        fuel = maxFuel;
    }

    void Crashed() {
        Debug.Log("crashed");
        SceneManager.LoadSceneAsync("GameOver");
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
            if(collision.otherCollider == forceFieldColl) {
                Destroy(collision.gameObject);
                debrisDestroyed++;
            }
            else if (collision.otherCollider == shipColl){
                Crashed();
            }
        }
    }


    private void OnDisable()
    {
        PlayerPrefs.SetInt("debrisDestroyed", debrisDestroyed);
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
    } 
}
