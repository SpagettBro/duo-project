using UnityEngine;

public class AlienDamage : MonoBehaviour
{
    public float damageAmount = 2f;
    public float damageInterval = 1f; // Time between each damage tick

    private bool canDamage = true;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    System.Collections.IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageInterval);
        canDamage = true;
    }
}
