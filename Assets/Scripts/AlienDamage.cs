using UnityEngine;

public class AlienDamage : MonoBehaviour
{
    public float damageAmount = 2f;
    public float damageInterval = 1f;

    private bool canDamage = true;
    private Animator anim;
    public float dashDistance = 0.2f;
    public float dashDuration = 0.3f; // Slightly slower dash

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
                if (anim != null)
                {
                    anim.SetTrigger("maleAttack");
                }

                StartCoroutine(PerformAttack(playerHealth, other.transform));
            }
        }
    }

    private System.Collections.IEnumerator PerformAttack(PlayerHealth playerHealth, Transform target)
    {
        canDamage = false;

        Vector3 startPosition = transform.position;
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 dashPosition = startPosition + direction * dashDistance;

        float elapsed = 0f;

        // Dash forward
        while (elapsed < dashDuration)
        {
            transform.position = Vector3.Lerp(startPosition, dashPosition, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = dashPosition;

        // 💥 Deal damage at the peak of dash
        playerHealth.TakeDamage(damageAmount);

        elapsed = 0f;

        // Dash back
        while (elapsed < dashDuration)
        {
            transform.position = Vector3.Lerp(dashPosition, startPosition, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;

        // Wait before next allowed damage
        yield return new WaitForSeconds(damageInterval);
        canDamage = true;
    }
}
