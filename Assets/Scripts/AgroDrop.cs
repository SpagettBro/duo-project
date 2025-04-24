using UnityEngine;

public class AgroDropChase : MonoBehaviour
{
    AudioManager audioManager;
    private bool hasChased = false;
    private bool hasDropped = false;

    [SerializeField] Transform player;
    [SerializeField] float agroRange = 9f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float dropSpeed = 5f;
    [SerializeField] float yThreshold = 0.1f;

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

        rb2d.gravityScale = 0; // No gravity for precise control
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
                if (audioManager != null && audioManager.wakeup != null)
                    audioManager.PlaySFX(audioManager.wakeup);

                hasChased = true;
            }

            if (!hasDropped)
            {
                if (Mathf.Abs(yDifference) > yThreshold)
                {
                    DropDown();
                }
                else
                {
                    LockYPosition();
                    hasDropped = true;
                }
            }
            else
            {
                ChasePlayerOnX();
            }
        }
    }

    void DropDown()
    {
        // Drop straight down
        rb2d.linearVelocity = new Vector2(0, -dropSpeed);

        // Flip sprite to face feet down
        transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    void LockYPosition()
    {
        rb2d.linearVelocity = Vector2.zero;
        transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);

        // Flip back to upright
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    void ChasePlayerOnX()
    {
        float directionX = player.position.x - transform.position.x;
        float moveX = Mathf.Sign(directionX) * moveSpeed;

        rb2d.linearVelocity = new Vector2(moveX, 0);

        // Flip sprite to face player
        if (directionX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(directionX) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
