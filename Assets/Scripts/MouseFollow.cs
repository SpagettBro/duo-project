using UnityEngine;

public class MouseFollowRotation2D : MonoBehaviour
{
    void Update()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f; // Ensure we're only working in 2D

        // Calculate the direction to the mouse
        Vector3 direction = mouseWorldPosition - transform.position;

        // Get the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the GameObject to face the mouse
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
