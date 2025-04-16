using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Flash Settings")]
    public SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    [Header("Invincibility Settings")]
    public float damageCooldown = 1f;
    private bool isInvincible = false;

    private Color originalColor;

    [Header("UI & Screen Shake")]
    public Slider healthSlider;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.2f;

    // This should be the ShakeContainer, not the actual camera
    public Transform shakeTarget;

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

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (shakeTarget == null)
        {
            // Try to find "ShakeContainer" by name
            GameObject shakeObj = GameObject.Find("ShakeContainer");
            if (shakeObj != null)
            {
                shakeTarget = shakeObj.transform;
            }
            else
            {
                Debug.LogWarning("ShakeContainer not found! Assign 'shakeTarget' in Inspector.");
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Current health: " + currentHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (shakeTarget != null)
        {
            StartCoroutine(ScreenShake());
        }

        StartCoroutine(FlashEffect());
        StartCoroutine(DamageCooldown());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log(gameObject.name + " healed by " + amount + ". Current health: " + currentHealth);
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

    IEnumerator ScreenShake()
    {
        Vector3 originalPos = shakeTarget.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector2 randomPoint = Random.insideUnitCircle * shakeMagnitude;
            shakeTarget.localPosition = new Vector3(randomPoint.x, randomPoint.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeTarget.localPosition = originalPos;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
