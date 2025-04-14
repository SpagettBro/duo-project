using UnityEngine;

public class MedkitPickup : MonoBehaviour
{
    public float healAmount = 2f; // Amount of HP to restore

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount); // Heal the player
            }

            Destroy(gameObject); // Remove the medkit after pickup
        }
    }
}
