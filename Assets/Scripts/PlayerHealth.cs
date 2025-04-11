using UnityEngine;
using UnityEngine.UI; // Needed for Slider
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Flash Settings")]
    public SpriteRenderer spriteRenderer; // Assign in Inspector
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    [Header("Invincibility Settings")]
    public float damageCooldown = 1f; // 1 second of invincibility after taking damage
    private bool isInvincible = false;

    private Color originalColor;

    // Reference to the HP Slider
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning("PlayerHealth: No SpriteRenderer assigned or found!");
        }

        // Set the slider's value to the player's max health at the start
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Current health: " + currentHealth);

        // Update the health slider when player takes damage
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Set the slider value to the current health
        }

        StartCoroutine(FlashEffect());
        StartCoroutine(DamageCooldown());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    IEnumerator DamageCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(damageCooldown);
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        // Quit the game when the player dies
        Application.Quit();

        // For the editor, you can simulate quitting by stopping the play mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
