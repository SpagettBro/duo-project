using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnvironmentalLight : MonoBehaviour
{
    public Light2D ceilingLight;  // Reference to the Light2D component
    public float lightRange = 5f;  // How far the light reaches (adjust as needed)
    public float damagePerSecond = 10f;  // Damage per second for any alien in the cone
    public float coneAngle = 60f;  // Angle of the cone (should be adjustable)
    public LayerMask alienLayer;  // The layer to filter for aliens

    void Start()
    {
        if (ceilingLight == null)
        {
            ceilingLight = GetComponentInChildren<Light2D>();
        }

        if (ceilingLight == null)
        {
            Debug.LogError("EnvironmentalLight script couldn't find a Light2D component! Assign it manually in the Inspector.");
        }
    }

    void Update()
    {
        if (ceilingLight != null && ceilingLight.enabled)
        {
            DamageAliensInCone();  // Check for and apply damage to aliens in the cone
        }
    }

    void DamageAliensInCone()
    {
        // The position of the light (starting point of the cone)
        Vector2 origin = ceilingLight.transform.position;
        // The direction the light is shining (downward)
        Vector2 direction = -ceilingLight.transform.up;

        // Loop through all aliens in the world
        Collider2D[] hitAliens = Physics2D.OverlapCircleAll(origin, lightRange, alienLayer);

        foreach (Collider2D alien in hitAliens)
        {
            // Get the direction from the light to the alien
            Vector2 toAlien = (alien.transform.position - (Vector3)origin).normalized;
            
            // Calculate the angle between the light's direction and the direction to the alien
            float angleToAlien = Vector2.Angle(direction, toAlien);

            // Check if the alien is within the cone angle (half the cone angle on each side)
            if (angleToAlien <= coneAngle / 2)
            {
                // Check if the alien is within the light's range
                float distanceToAlien = Vector2.Distance(origin, alien.transform.position);
                if (distanceToAlien <= lightRange)
                {
                    // Apply damage over time while the alien is inside the cone
                    AlienHealth alienHealth = alien.GetComponent<AlienHealth>();
                    if (alienHealth != null)
                    {
                        alienHealth.TakeDamage(damagePerSecond * Time.deltaTime);
                        Debug.Log($"Alien {alien.name} is within the cone and taking damage!");
                    }
                }
            }
            else
            {
                // Debugging log for aliens outside the cone
                Debug.Log($"Alien {alien.name} is outside the cone.");
            }
        }
    }

    void OnDrawGizmos()
    {
        // Only draw the cone if the light is properly assigned
        if (ceilingLight == null) return;

        Gizmos.color = Color.yellow;
        Vector3 lightPos = ceilingLight.transform.position;
        Vector3 leftLimit = Quaternion.Euler(0, 0, -coneAngle / 2) * -ceilingLight.transform.up * lightRange;
        Vector3 rightLimit = Quaternion.Euler(0, 0, coneAngle / 2) * -ceilingLight.transform.up * lightRange;

        // Visualize the cone in the Scene view for debugging purposes
        Gizmos.DrawLine(lightPos, lightPos + leftLimit);
        Gizmos.DrawLine(lightPos, lightPos + rightLimit);
    }
}
