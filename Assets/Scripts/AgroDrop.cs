using UnityEngine;

public class AgroDropChase : MonoBehaviour
{
    AudioManager audioManager;
    private bool hasChased = false; // Flag to track if the alien has started chasing

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    [SerializeField] Transform player;
    [SerializeField] float agroRange = 9f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float dropSpeed = 5f;
    [SerializeField] float yThreshold = 0.1f; // Distance before stopping drop

    Rigidbody2D rb2d;

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
        rb2d.linearVelocity = new Vector2(0, -dropSpeed); // Move straight down
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb2d.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }
}
