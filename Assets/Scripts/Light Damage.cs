using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternLight : MonoBehaviour
{
    public Light2D lanternLight; // The Light2D component
    public float lightRange = 5.2f; // How far the light reaches
    public float damagePerSecond = 10f; // Damage applied per second
    public float coneAngle = 60f; // The angle of the cone
    public LayerMask alienLayer; // Define the layer for aliens

    private Transform player;  // Reference to the player
    private float playerFacingDirection;  // Player's facing direction (1 for right, -1 for left)

    void Start()
    {
        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player").transform; 

        // If the light hasn't been assigned in the Inspector, try to find it automatically
        if (lanternLight == null)
        {
            lanternLight = GetComponentInChildren<Light2D>();
        }

        if (lanternLight == null)
        {
            Debug.LogError("LanternLight script couldn't find a Light2D component! Assign it manually in the Inspector.");
        }
    }

    void Update()
    {
        if (lanternLight != null && lanternLight.enabled)
        {
            // Get the player's facing direction from PlayerController
            playerFacingDirection = GetPlayerFacingDirection();

            // Update damage area based on the direction the player is facing
            DamageAliensInCone();
        }
    }

    void DamageAliensInCone()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right.normalized; // Default direction (forward of lantern)

        // If player is facing left, invert the direction of the cone
        if (playerFacingDirection < 0)
        {
            direction = -transform.right.normalized; // Flip the direction to the left
        }

        // Use OverlapArea to check for aliens in the cone's range
        Collider2D[] hitAliens = Physics2D.OverlapAreaAll(origin, origin + direction * lightRange, alienLayer);

        foreach (Collider2D alien in hitAliens)
        {
            Vector2 toAlien = (alien.transform.position - (Vector3)origin).normalized;
            float angleToAlien = Vector2.Angle(direction, toAlien);

            // Check if alien is inside the cone angle
            if (angleToAlien <= coneAngle / 2)
            {
                AlienHealth alienHealth = alien.GetComponent<AlienHealth>();
                if (alienHealth != null)
                {
                    alienHealth.TakeDamage(damagePerSecond * Time.deltaTime);
                    Debug.Log($"Alien {alien.name} is taking damage!");
                }
            }
        }
    }

    // Function to check the player's facing direction (from PlayerController)
    float GetPlayerFacingDirection()
    {
        // Assuming PlayerController flips the player's sprite using localScale.x
        return Mathf.Sign(player.localScale.x); // Return 1 for right, -1 for left
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftLimit = Quaternion.Euler(0, 0, -coneAngle / 2) * transform.right * lightRange;
        Vector3 rightLimit = Quaternion.Euler(0, 0, coneAngle / 2) * transform.right * lightRange;

        // Draw the cone in the Scene view for debugging
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
    }
}
