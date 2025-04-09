using UnityEngine;

public class AlienHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Damage Effects")]
    public ParticleSystem damageParticles; // Reference to the particle system

    private AudioManager audioManager; // Reference to the AudioManager

    void Start()
    {
        currentHealth = maxHealth;

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
        Debug.Log(gameObject.name + " has been defeated!");
        Destroy(gameObject);
    }
}
