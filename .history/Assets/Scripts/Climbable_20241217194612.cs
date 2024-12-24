using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false; // Player is in the ladder area
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f; // Adjustable climbing speed
    public float climbingAcceleration = 8f; // Smooth acceleration

    private float currentClimbingVelocity = 0f; // For smooth velocity transition
    private float smoothTime = 0.1f; // Smoothing time for climbing

    void Update()
    {
        if (isOnLadder && Input.GetKeyDown(KeyCode.E))
        {
            var cat = playerRigidbody.gameObject.GetComponent<Cat>();
            if (cat.isClimbing)
            {
                StopClimbing(); // Drop from ladder
            }
            else
            {
                StartClimbing(); // Start climbing
            }
        }

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
            StopClimbing(); // Ensure climbing stops when exiting
        }
    }

    private void StartClimbing()
    {
        if (playerRigidbody != null)
        {
            var cat = playerRigidbody.gameObject.GetComponent<Cat>();
            cat.isClimbing = true;
            playerRigidbody.gravityScale = 0; // Disable gravity
            playerRigidbody.velocity = Vector2.zero; // Reset previous momentum
            currentClimbingVelocity = 0f; // Reset climbing velocity for smooth start
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
            }
        }
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxisRaw("Vertical"); // Get W/S or Up/Down input
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, verticalInput * climbingSpeed); // Apply vertical movement
    }
}
