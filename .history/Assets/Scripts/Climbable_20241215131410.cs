using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    private bool isClimbing = false;
    private Rigidbody2D playerRigidbody;

    // Update is called once per frame
    void Update()
    {
        if (isClimbing && Input.GetAxisRaw("Vertical") != 0)
        {
            Climb();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Enable climbing
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 0; 
                isClimbing = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Disable climbing
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 1;
                isClimbing = false;
            }
        }
    }

    private void Climb()
    {
        float vertical = Input.GetAxisRaw("Vertical"); 
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, vertical * 5f); // Adjust climbing speed as needed
    }
}
