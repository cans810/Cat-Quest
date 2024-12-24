using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false; // Player is in the ladder area
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 6f; // Adjustable climbing speed for smooth and faster climbing
    public float climbingAcceleration = 10f; // Smoothing factor

    private Vector2 climbVelocity;

    void Update()
    {
        if (isOnLadder && Input.GetKeyDown(KeyCode.E))
        {
            if (playerRigidbody != null && playerRigidbody.gameObject.GetComponent<Cat>().isClimbing)
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
            playerRigidbody.gameObject.GetComponent<Cat>().isClimbing = true;
            playerRigidbody.gravityScale = 0; // Disable gravity
            playerRigidbody.velocity = Vector2.zero; // Stop any previous momentum
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null && playerRigidbody.gameObject.GetComponent<Cat>().isClimbing)
        {
            playerRigidbody.gameObject.GetComponent<Cat>().isClimbing = false;
            playerRigidbody.gravityScale = 2.5f; // Re-enable gravity
        }
    }

    private void HandleClimbing()
    {
        float vertical = Input.GetAxis("Vertical"); // W (1) or S (-1)

        // Smooth movement using interpolation
        float targetYVelocity = vertical * climbingSpeed;
        climbVelocity.y = Mathf.Lerp(climbVelocity.y, targetYVelocity, climbingAcceleration * Time.deltaTime);

        playerRigidbody.velocity = new Vector2(0, climbVelocity.y); // Apply climbing velocity
    }
}
