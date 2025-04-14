using UnityEngine;

public class Agro : MonoBehaviour
{
    AudioManager audioManager;

    private bool hasChased = false; // Flag to track if the alien has started chasing

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }

        // Calculate distance to player
        float distToPLayer = Vector2.Distance(transform.position, player.position);

        if (distToPLayer < agroRange)
        {
            // Code to chase player
            if (!hasChased) // If the alien hasn't started chasing yet
            {
                if (audioManager.wakeup != null) // Check if the clip is assigned before playing
                {
                    audioManager.PlaySFX(audioManager.wakeup); // Play the sound once
                }
                else
                {
                    Debug.LogError("alien_wakeup is still not assigned!");
                }

                hasChased = true; // Set the flag to true to avoid playing the sound again
            }
            ChasePlayer();
        }
        else
        {
            // Alien is out of range, but we don't reset hasChased here anymore
            // No need to reset the flag, as we want the sound to play only once when the chase starts.
        }
    }
    void ChasePlayer()
{
    // Calculate the direction from the alien to the player
    Vector2 direction = (player.position - transform.position).normalized;

    // Update the Rigidbody2D's linearVelocity to move the alien towards the player
    rb2d.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
}

}
