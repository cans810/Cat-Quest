using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false; // Tracks if player is in ladder area
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f; // Adjustable climbing speed
    public float climbingAcceleration = 0.1f; // Smoothing for climbing velocity
    public BoxCollider2D playerCollider; // Reference to the player's BoxCollider2D
    public float liftOffset = 0.1f; // Small vertical offset to lift the player

    private float currentClimbingVelocity = 0f;
    private float ladderXPosition; // X-position of the ladder

    private bool isClimbing = false; // Tracks the climbing state

    void Update()
    {
        // Check if the player presses E to start/stop climbing
        if (isOnLadder && Input.GetKeyDown(KeyCode.E))
        {
            ToggleClimbing();
        }

        // If the player is climbing, handle climbing movement
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
            playerCollider = collision.GetComponent<BoxCollider2D>();
            ladderXPosition = transform.position.x; // Set ladder X-position
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = false;

            // Only stop climbing if the player isn't actively climbing
            if (!isClimbing)
            {
                StopClimbing();
            }
        }
    }

    private void ToggleClimbing()
    {
        if (!isClimbing)
        {
            StartClimbing();
        }
        else
        {
            StopClimbing();
        }
    }

    private void StartClimbing()
    {
        if (playerRigidbody != null && playerCollider != null)
        {
            isClimbing = true;
            playerRigidbody.gravityScale = 0; // Disable gravity while climbing
            playerRigidbody.velocity = Vector2.zero; // Reset velocity for smooth start
            playerCollider.enabled = false; // Disable player's collider
            playerRigidbody.position += new Vector2(0, liftOffset); // Slightly lift the player
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null && playerCollider != null)
        {
            isClimbing = false;
            playerRigidbody.gravityScale = 2.5f; // Restore gravity
            playerCollider.enabled = true; // Re-enable player's collider
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f); // Stop vertical movement
        }
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Smoothly adjust climbing velocity
        currentClimbingVelocity = Mathf.Lerp(currentClimbingVelocity, verticalInput * climbingSpeed, climbingAcceleration);

        // Stick the player to the ladder's X-position
        playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y);

        // Apply climbing movement
        playerRigidbody.velocity = new Vector2(0f, currentClimbingVelocity);
    }
}
