using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MultiplanetOrbiter
{
    
    [SerializeField] private float groundSpeed = 6f;
    [SerializeField] private float gravStrength = 0.0015f;
    [SerializeField] private float rocketPower = 0.008f;
    [SerializeField] private float maxFloatSpeed = 0.1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float maxFuel = 300f;
    

    private float bottomOfShip = 0.865f;

    private float startTime;


    // sprites
    [SerializeField] private Sprite baseSprite, leftSprite, rightSprite, noSprite, leftSmallSprite, rightSmallSprite, pullSprite, pushSprite;

    [SerializeField] private Slider slider;
    [SerializeField] private Collider2D shipColl;
    [SerializeField] private Collider2D forceFieldColl;


    private SpriteRenderer spriteRenderer;

    private GameObject forceField;

    AudioController audioController;

    private GameObject pull;

    private GameObject push;

    private float fuel;

    [SerializeField]
    private Spawner spawner;

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

    new void Awake()
    {
        base.Awake();

        try {
            audioController = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioController>();
        }
        catch {
            Debug.LogError("No audio controller found");
        }
        
    }

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = State.Floating;
        forceField = transform.GetChild(0).gameObject;
        push = transform.GetChild(1).gameObject;
        pull = transform.GetChild(2).gameObject;
        fuel = maxFuel;
        ppState = PushPullState.None;
        startTime = Time.time;
        audioController.StartMusic();
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
        Vector2 rotatedPos = Quaternion.AngleAxis(-hori * groundSpeed / planetToShip.magnitude, Vector3.forward) * planetToShip.normalized * (planetRadius + (bottomOfShip/2));

        rb.MovePosition(rotatedPos + (Vector2) planet.transform.position);
        transform.up = rotatedPos;

        if(vert == 1) 
        {
            velocity = 17 * rocketPower * planetToShip.normalized; 
            UpdatePosition();
        }

    }

    void HandleForceField() {
        if(Input.GetKey(KeyCode.LeftShift) && fuel > 0) {
            forceField.SetActive(true);
            fuel -= 0.5f;
            audioController.PlayForcefield = true;
        }
        else {
            forceField.SetActive(false);
            audioController.PlayForcefield = false;
        }
    }

    void HandlePushPull() {
        if(Input.GetKey(KeyCode.Z) && fuel > 0) {
            pull.SetActive(true);
            push.SetActive(false);
            ppState = PushPullState.Pulling;
            fuel -= 0.25f;
            audioController.PlayPull = true;
            audioController.PlayPush = false;
        }
        else if (Input.GetKey(KeyCode.X) && fuel > 0) {
            push.SetActive(true);
            pull.SetActive(false);
            ppState = PushPullState.Pushing;
            fuel -= 0.25f;
            audioController.PlayPull = false;
            audioController.PlayPush = true;
        }
        else {
            ppState = PushPullState.None;
            pull.SetActive(false);
            push.SetActive(false);
            audioController.PlayPull = false;
            audioController.PlayPush = false;
        }
    }

    void FloatingMovement() 
    {
        float vert = Math.Max(Input.GetAxisRaw("Vertical"), 0); // Remove back movement
        float hori = Input.GetAxisRaw("Horizontal");
        
        if(fuel > 0) {
            velocity += rocketPower * vert * (Vector2) transform.up;
            transform.Rotate(0, 0, -rotateSpeed * hori, Space.Self);
        }

        if (vert != 1 || fuel <= 0) 
        {
            Vector2 planetToShip = transform.position - planet.transform.position;
            velocity += -gravStrength * planetToShip.normalized;
        }
 
        velocity = Vector2.ClampMagnitude(velocity, maxFloatSpeed);

        print(velocity);


        UpdatePosition();

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

        if(vert == 0 && hori != 0 && fuel > 0) 
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

        audioController.PlayEngine = (vert != 0 || hori !=0) && fuel > 0;
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
        else {
            audioController.PlaySFX(audioController.land);
        }
        velocity = new();
        transform.up = planetToShip;
        Vector2 shadow = planetToShip.normalized * (planetRadius + (bottomOfShip/2));
        transform.position = new Vector3(shadow.x, shadow.y, -1) + planet.transform.position;

        spriteRenderer.sprite = noSprite;

        fuel = maxFuel;
    }

    void Crashed() {
        Debug.Log("crashed");
        audioController.PlaySFX(audioController.destroyed);
        audioController.StopAll();

        PlayerPrefs.SetString("time", TimeSpan.FromSeconds((double) Time.time - startTime).ToString("mm\\:ss"));

        SceneManager.LoadSceneAsync("GameOver");
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        print(other.gameObject.tag);
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
                spawner.SpawnRandom();
                audioController.PlaySFX(audioController.killedJunk);
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
