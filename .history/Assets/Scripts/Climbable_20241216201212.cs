using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false; // Player is in the ladder area
     // Player is actively climbing
    public Rigidbody2D playerRigidbody;

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
            playerRigidbody.gravityScale = 1; // Re-enable gravity
        }
    }

    private void HandleClimbing()
    {
        float vertical = Input.GetAxisRaw("Vertical"); // W (1) or S (-1)

        if (vertical != 0)
        {
            playerRigidbody.velocity = new Vector2(0, vertical * 5f); // Move only vertically
        }
        else
        {
            playerRigidbody.velocity = new Vector2(0, 0); // Stop vertical movement when no input
        }
    }
}
