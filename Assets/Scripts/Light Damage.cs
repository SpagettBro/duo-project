using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternLight : MonoBehaviour
{
    public Light2D lanternLight; // The spotlight component of the lantern
    public float lightRange = 5.2f; // How far the light reaches
    public float damagePerSecond = 10f; // Damage applied per second
    public LayerMask alienLayer; // Define the layer for aliens
    public bool isRaysActive = false; // Toggle for rays or circle
    public int numberOfRays = 5; // Number of rays to emit
    public float rayWidth = 0.1f; // Width of each light ray

    private LineRenderer[] lightRays; // Array of LineRenderers for rays

    void Start()
    {
        // Find the Light2D component inside the Lantern object if not assigned
        if (lanternLight == null)
        {
            lanternLight = GetComponentInChildren<Light2D>();
        }

        if (lanternLight == null)
        {
            Debug.LogError("LanternLight script couldn't find a Light component! Assign it manually in the Inspector.");
        }

        // Initialize rays if not already created
        lightRays = new LineRenderer[numberOfRays];
        for (int i = 0; i < numberOfRays; i++)
        {
            lightRays[i] = new GameObject("Ray_" + i).AddComponent<LineRenderer>();
            lightRays[i].transform.parent = transform;
            lightRays[i].startWidth = rayWidth;
            lightRays[i].endWidth = rayWidth;
            lightRays[i].startColor = Color.yellow;
            lightRays[i].endColor = Color.yellow;
            lightRays[i].enabled = false;  // Initially disable the rays
        }
    }

    void Update()
    {
        // Only damage when the light is on
        if (lanternLight != null && lanternLight.enabled)
        {
            DamageAliensInLight();
            if (isRaysActive)
            {
                UpdateRays();
            }
        }

        // Toggle between rays and circle light (you can link this with a button or input)
        if (Input.GetKeyDown(KeyCode.T)) // Change T to whatever key you want
        {
            ToggleLightMode();
        }
    }

    void DamageAliensInLight()
    {
        // Check for aliens within the range of the light
        Collider2D[] hitAliens = Physics2D.OverlapCircleAll(transform.position, lightRange, alienLayer);

        foreach (Collider2D alien in hitAliens)
        {
            AlienHealth alienHealth = alien.GetComponent<AlienHealth>();
            if (alienHealth != null)
            {
                alienHealth.TakeDamage(damagePerSecond * Time.deltaTime); // Apply damage over time
            }
        }
    }

    void UpdateRays()
    {
        // Calculate the angle between rays and update their positions
        float angleStep = 360f / numberOfRays;  // Angle between each ray
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

            lightRays[i].SetPosition(0, transform.position);  // Start point (center of lantern)
            lightRays[i].SetPosition(1, transform.position + direction * lightRange);  // End point (ray length)
        }
    }

    void ToggleLightMode()
    {
        // Toggle between rays and circle light
        isRaysActive = !isRaysActive;
        lanternLight.enabled = !isRaysActive;  // Disable lantern circle if rays are active

        // Enable or disable rays based on the toggle state
        for (int i = 0; i < numberOfRays; i++)
        {
            lightRays[i].enabled = isRaysActive;
        }
    }

    void OnDrawGizmos()
    {
        // Visualize the range of the lantern light
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lightRange);
    }
}
