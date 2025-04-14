using UnityEngine;

public class AlienDamage : MonoBehaviour
{
    public float damageAmount = 2f;
    public float damageInterval = 1f; // Time between each damage tick

    private bool canDamage = true;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Damage the player
                playerHealth.TakeDamage(damageAmount);

                // Trigger attack animation ONLY when damaging
                if (anim != null)
                {
                    anim.SetTrigger("maleAttack");
                }

                // Start cooldown
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private System.Collections.IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageInterval);
        canDamage = true;
    }
}
