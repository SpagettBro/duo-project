using System;
using UnityEditor.Search;
using UnityEngine;

public class Agro : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            return;
        }


        //distance to player
        float distToPLayer = Vector2.Distance(transform.position, player.position);
        
        if(distToPLayer < agroRange)
        {
            //code to chase player
            ChasePlayer();
        }
        else
        {
            //stop chasing player
        }
    }

    void ChasePlayer()
    {
        if(transform.position.x < player.position.x)
        {
            //enemy is to the left side of the player, so move right
            rb2d.linearVelocity = new Vector2(moveSpeed, rb2d.linearVelocity.y);
        }
        else
        {
            //enemy is to the right side of the player, so move left
             rb2d.linearVelocity = new Vector2(-moveSpeed, rb2d.linearVelocity.y);
        }
    }
}
