using UnityEngine;

public class AlienHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // Maximum health for the alien
    private float currentHealth;   // Current health of the alien

    void Start()
    {
        currentHealth = maxHealth;  // Set starting health to max
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;  // Decrease health by damage amount

        // Log when alien takes damage
        Debug.Log(gameObject.name + " took " + amount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();  // If health reaches 0, call Die()
        }
    }

    void Die()
    {
        // Log when alien dies
        Debug.Log(gameObject.name + " has been defeated!");

        // Destroy alien game object
        Destroy(gameObject);
    }
}
