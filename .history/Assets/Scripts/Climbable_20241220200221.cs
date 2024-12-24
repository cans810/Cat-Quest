using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false; // Player is in the ladder area
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f; // Adjustable climbing speed
    public float climbingAcceleration = 0.1f; // Smoothing for climbing velocity

    private float currentClimbingVelocity = 0f;
    private float inputVertical;

    void Update()
    {
        // Check if the player presses E to start/stop climbing
        if (isOnLadder && Input.GetKeyDown(KeyCode.E))
        {
            ToggleClimbing();
        }

        // If the player is climbing, handle climbing movement
        if (playerRigidbody != null && playerRigidbody.gameObject.GetComponent<Cat>().isClimbing)
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
            isOnLadder = false;
            StopClimbing();
        }
    }

    private void ToggleClimbing()
    {
        var cat = playerRigidbody.gameObject.GetComponent<Cat>();
        if (cat.isClimbing)
        {
            StopClimbing();
        }
        else
        {
            StartClimbing();
        }
    }

    private void StartClimbing()
    {
        if (playerRigidbody != null)
        {
            var cat = playerRigidbody.gameObject.GetComponent<Cat>();
            cat.isClimbing = true;
            playerRigidbody.gravityScale = 0; // Disable gravity while climbing
            playerRigidbody.velocity = Vector2.zero; // Reset velocity for smooth start
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null)
        {
            var cat = playerRigidbody.gameObject.GetComponent<Cat>();
            if (cat.isClimbing)
            {
                cat.isClimbing = false;
                playerRigidbody.gravityScale = 2.5f; // Restore normal gravity
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f); // Stop vertical movement
            }
        }
    }

    private void HandleClimbing()
    {
        // Get vertical input
        inputVertical = Input.GetAxisRaw("Vertical");

        // Smoothly adjust climbing velocity
        currentClimbingVelocity = Mathf.Lerp(currentClimbingVelocity, inputVertical * climbingSpeed, climbingAcceleration);

        // Apply climbing movement
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, currentClimbingVelocity);
    }
}
