using System;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    public float climbSpeed = 3f;
    private Rigidbody2D rb;
    private bool isClimbing = false;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical"); // Arrow keys or W/S

        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Reached climbable object.");
        if (other.CompareTag("Climbable"))
        {
            isClimbing = true;
            rb.gravityScale = 0f; // Disable gravity while climbing
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Left climbable object.");
        if (other.CompareTag("Climbable"))
        {
            isClimbing = false;
            rb.gravityScale = 1f; // Restore gravity
        }
    }
}
