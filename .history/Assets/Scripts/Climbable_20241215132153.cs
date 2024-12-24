using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    private bool isClimbing = false;
    private bool isOnLadder = false;
    private Rigidbody2D playerRigidbody;

    void Update()
    {
        // Allow climbing only when player is on the ladder and pressing E
        if (isOnLadder && Input.GetKey(KeyCode.E))
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing && Input.GetAxisRaw("Vertical") != 0)
        {
            Climb();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player has entered the ladder area
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                isOnLadder = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player has exited the ladder area
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 1; // Restore gravity
                isClimbing = false;
                isOnLadder = false;
            }
        }
    }

    private void Climb()
    {
        float vertical = Input.GetAxisRaw("Vertical"); // Use W/S or Up/Down arrow keys
        playerRigidbody.gravityScale = 0; // Disable gravity while climbing
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, vertical * 5f); // Adjust climbing speed as needed
    }
}
