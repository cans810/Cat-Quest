using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false;
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f;
    public BoxCollider2D playerCollider;
    public float liftOffset = 0.1f;

    private bool isClimbing = false;
    private float ladderXPosition;

    void Update()
    {
        if (isOnLadder && Input.GetKeyDown(KeyCode.E))
        {
            if (isClimbing)
            {
                StopClimbing();
            }
            else
            {
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
            playerCollider = collision.GetComponent<BoxCollider2D>();
            ladderXPosition = transform.position.x; // Center of the ladder
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = false;

            // Stop climbing when leaving the ladder
            if (isClimbing)
            {
                StopClimbing();
            }
        }
    }

    private void StartClimbing()
    {
        if (playerRigidbody != null)
        {
            isClimbing = true;
            playerRigidbody.gravityScale = 0;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y + liftOffset);
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null)
        {
            isClimbing = false;
            playerRigidbody.gravityScale = 2.5f; // Reset gravity
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f);
        }
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Control vertical movement
        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            playerRigidbody.velocity = new Vector2(0f, verticalInput * climbingSpeed);
        }
        else
        {
            // Stop vertical movement when no input
            playerRigidbody.velocity = Vector2.zero;
        }

        // Keep the player centered on the ladder
        playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y);

        // Allow exiting the ladder with horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            StopClimbing();
        }
    }
}
