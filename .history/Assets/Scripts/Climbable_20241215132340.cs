using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    private bool isOnLadder = false; // Player is in the ladder area
    private bool isClimbing = false; // Player is actively climbing
    private Rigidbody2D playerRigidbody;

    void Update()
    {
        if (isOnLadder && Input.GetKeyDown(KeyCode.E))
        {
            if (isClimbing)
            {
                // If already climbing, pressing E drops the player
                StopClimbing();
            }
            else
            {
                // Start climbing
                StartClimbing();
            }
        }

        if (isClimbing)
        {
            HandleClimbing();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = true;
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player leaves ladder zone
            isOnLadder = false;
            StopClimbing(); // Ensure climbing is stopped
        }
    }

    private void StartClimbing()
    {
        if (playerRigidbody != null)
        {
            isClimbing = true;
            playerRigidbody.gravityScale = 0; // Disable gravity while climbing
            playerRigidbody.velocity = Vector2.zero; // Stop any previous momentum
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null)
        {
            isClimbing = false;
            playerRigidbody.gravityScale = 1; // Re-enable gravity
        }
    }

    private void HandleClimbing()
    {
        float vertical = Input.GetAxisRaw("Vertical"); // W (1) or S (-1) input

        if (vertical != 0)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, vertical * 5f); // Adjust speed
        }
        else
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0); // Stop vertical movement when no input
        }
    }
}
