using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnvironmentalLight : MonoBehaviour
{
    public Light2D ceilingLight;  // Reference to the Light2D component
    public float lightRange = 5f;  // How far the light reaches
    public float coneAngle = 60f;  // Angle of the cone
    public LayerMask alienLayer;  // Layer to filter for aliens

    public float tickInterval = 0.3f; // Time between damage ticks
    public float damagePerTick = 1f;  // Damage dealt per tick

    private float tickTimer = 0f;

    // Track cooldown per alien so they only take damage per tick
    private Dictionary<GameObject, float> alienTickTimers = new Dictionary<GameObject, float>();

    void Start()
    {
        if (ceilingLight == null)
        {
            ceilingLight = GetComponentInChildren<Light2D>();
        }

        if (ceilingLight == null)
        {
            Debug.LogError("EnvironmentalLight script couldn't find a Light2D component!");
        }
    }

    void Update()
    {
        if (ceilingLight != null && ceilingLight.enabled)
        {
            tickTimer += Time.deltaTime;

            if (tickTimer >= tickInterval)
            {
                DamageAliensInCone();
                tickTimer = 0f;
            }
        }
    }

    void DamageAliensInCone()
    {
        Vector2 origin = ceilingLight.transform.position;
        Vector2 direction = -ceilingLight.transform.up;

        Collider2D[] hitAliens = Physics2D.OverlapCircleAll(origin, lightRange, alienLayer);

        foreach (Collider2D alien in hitAliens)
        {
            Vector2 toAlien = (alien.transform.position - (Vector3)origin).normalized;
            float angleToAlien = Vector2.Angle(direction, toAlien);

            if (angleToAlien <= coneAngle / 2)
            {
                float distanceToAlien = Vector2.Distance(origin, alien.transform.position);
                if (distanceToAlien <= lightRange)
                {
                    AlienHealth alienHealth = alien.GetComponent<AlienHealth>();
                    if (alienHealth != null)
                    {
                        alienHealth.TakeDamage(damagePerTick);
                        Debug.Log($"Alien {alien.name} is within the cone and took {damagePerTick} tick damage.");
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (ceilingLight == null) return;

        Gizmos.color = Color.yellow;
        Vector3 lightPos = ceilingLight.transform.position;
        Vector3 leftLimit = Quaternion.Euler(0, 0, -coneAngle / 2) * -ceilingLight.transform.up * lightRange;
        Vector3 rightLimit = Quaternion.Euler(0, 0, coneAngle / 2) * -ceilingLight.transform.up * lightRange;

        Gizmos.DrawLine(lightPos, lightPos + leftLimit);
        Gizmos.DrawLine(lightPos, lightPos + rightLimit);
    }
}
