using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class LanternLight : MonoBehaviour
{
    public Light2D lanternLight; // The Light2D component
    public float lightRange = 5.2f; // How far the light reaches
    public float tickInterval = 0.5f; // Time between damage ticks


    public float coneAngle = 60f; // The angle of the cone
    public LayerMask alienLayer; // Define the layer for aliens

    private Transform player;  // Reference to the player
    private float playerFacingDirection;  // Player's facing direction (1 for right, -1 for left)


    // Keeps track of when each alien was last damaged
    private Dictionary<Collider2D, float> lastDamageTime = new Dictionary<Collider2D, float>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
            playerFacingDirection = GetPlayerFacingDirection();
            DamageAliensInCone();
        }
    }

    void DamageAliensInCone()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right.normalized;

        if (playerFacingDirection < 0)
        {
            direction = -transform.right.normalized;
        }

        Collider2D[] hitAliens = Physics2D.OverlapCircleAll(origin, lightRange, alienLayer);

        foreach (Collider2D alien in hitAliens)
        {
            Vector2 toAlien = (alien.transform.position - (Vector3)origin).normalized;
            float angleToAlien = Vector2.Angle(direction, toAlien);

            if (angleToAlien <= coneAngle / 2)
            {
                // Only apply damage if enough time has passed for this alien
                if (!lastDamageTime.ContainsKey(alien) || Time.time - lastDamageTime[alien] >= tickInterval)
                {
                    AlienHealth alienHealth = alien.GetComponent<AlienHealth>();
                    if (alienHealth != null)
                    {
                        alienHealth.TakeDamage(1); // Tick damage = 1
                        Debug.Log($"Alien {alien.name} took 1 damage from lantern tick!");
                    }

                    lastDamageTime[alien] = Time.time;
                }
            }
        }

        // Optionally clear out old entries to avoid growing dictionary endlessly
        CleanUpOldEntries();
    }

    float GetPlayerFacingDirection()
    {
        return Mathf.Sign(player.localScale.x);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftLimit = Quaternion.Euler(0, 0, -coneAngle / 2) * transform.right * lightRange;
        Vector3 rightLimit = Quaternion.Euler(0, 0, coneAngle / 2) * transform.right * lightRange;

        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
    }

    // Removes alien entries that are no longer valid (e.g. destroyed)
    void CleanUpOldEntries()
    {
        List<Collider2D> toRemove = new List<Collider2D>();
        foreach (var kvp in lastDamageTime)
        {
            if (kvp.Key == null)
            {
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
        {
            lastDamageTime.Remove(key);
        }
    }
}
