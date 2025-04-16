using UnityEngine;

public class Agro : MonoBehaviour
{
    AudioManager audioManager;
    private bool hasChased = false;
    private bool isChasing = false;

    [SerializeField] Transform player;
    [SerializeField] float agroRange = 5f;
    [SerializeField] float moveSpeed = 3f;

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
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        // Trigger aggro once
        if (!isChasing && distToPlayer < agroRange)
        {
            isChasing = true;

            if (!hasChased)
            {
                if (audioManager != null && audioManager.wakeup != null)
                {
                    audioManager.PlaySFX(audioManager.wakeup);
                }

                hasChased = true;
            }
        }

        // Chase if aggro'd
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Move
        Vector2 move = direction * moveSpeed;
        rb2d.MovePosition(rb2d.position + move * Time.fixedDeltaTime);

        // Flip sprite
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1f, 1f);
        }
    }
}
