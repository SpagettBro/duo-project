using UnityEngine;

public class AgroDropChase : MonoBehaviour
{
    AudioManager audioManager;
    private bool hasChased = false; // Flag to track if the alien has started chasing

    [SerializeField] Transform player;
    [SerializeField] float agroRange = 9f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float dropSpeed = 5f;
    [SerializeField] float yThreshold = 0.1f; // Distance before stopping drop

    Rigidbody2D rb2d;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);
        }
        rb2d.gravityScale = 0; // Ensure gravity is off
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        float yDifference = transform.position.y - player.position.y;

        if (distToPlayer < agroRange)
        {
            if (!hasChased)
            {
                if (audioManager.wakeup != null)
                {
                    audioManager.PlaySFX(audioManager.wakeup);
                }
                else
                {
                    Debug.LogError("alien_wakeup is not assigned!");
                }
                hasChased = true;
            }

            // If the enemy is too high, drop down first
            if (yDifference > yThreshold)
            {
                DropDown();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    void DropDown()
    {
        // Set only the vertical velocity for dropping down (no horizontal movement)
        rb2d.linearVelocity = new Vector2(0, -dropSpeed); // Move straight down with set speed
        
        // Flip the enemy's sprite to face the ground (feet down)
        transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Apply a force to move the enemy towards the player (no vertical movement)
        rb2d.AddForce(direction * moveSpeed, ForceMode2D.Force);

        // Flip the enemy's sprite to face the player (on the X-axis)
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
