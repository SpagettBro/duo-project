using UnityEngine;

public class AlienHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Damage Effects")]
    public ParticleSystem damageParticles; // Reference to the particle system

    private AudioManager audioManager; // Reference to the AudioManager
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDying = false;

    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Get AudioManager from the scene
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogWarning("AudioManager not found in scene! Make sure the Audio GameObject is tagged as 'Audio'.");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDying) return;

        currentHealth -= amount;

        // Play the damage particle effect if assigned
        if (damageParticles != null)
        {
            damageParticles.Play();
        }
        else
        {
            Debug.LogWarning("No damageParticles assigned on " + gameObject.name);
        }

        // Play damage sound effect if assigned
        if (audioManager != null && audioManager.alien_damaged != null)
        {
            audioManager.PlaySFX(audioManager.alien_damaged);
        }

        Debug.Log(gameObject.name + " took " + amount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
        {
            isDying = true;

            Debug.Log(gameObject.name + " has been defeated!");

            // Stop movement and physics
            if (rb != null)
            {
                rb.Sleep(); // Stops velocity and physics
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            // Disable damage and movement scripts
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                if (script != this) // Keep AlienHealth alive for animation & destroy timing
                {
                    script.enabled = false;
                }
            }

            // Trigger death animation
            if (anim != null)
            {
                anim.SetTrigger("Death");
            }

            // Destroy after delay to let animation finish
            Destroy(gameObject, 0.5f);
        }

}
