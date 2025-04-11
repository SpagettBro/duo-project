using UnityEngine;
using UnityEngine.SceneManagement;
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
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Current health: " + currentHealth);

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

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}
