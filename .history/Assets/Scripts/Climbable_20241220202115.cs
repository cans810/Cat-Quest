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
        // Only allow entering climbing state with E
        if (isOnLadder && Input.GetKeyDown(KeyCode.E) && !isClimbing)
        {
            StartClimbing();
        }

        // Handle climbing movement if we're climbing
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
            ladderXPosition = transform.position.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = false;
            if (isClimbing)
            {
                StopClimbing();
            }
        }
    }

    private void StartClimbing()
    {
        if (playerRigidbody != null && playerCollider != null)
        {
            isClimbing = true;
            playerRigidbody.gravityScale = 0;
            playerRigidbody.velocity = Vector2.zero;
            // Don't disable the collider anymore
            playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y + liftOffset);
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null)
        {
            isClimbing = false;
            playerRigidbody.gravityScale = 2.5f;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f);
        }
    }

    private void HandleClimbing()
    {
        float verticalInput = 0f;
        
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        if (verticalInput != 0f)
        {
            // Keep player centered on ladder while climbing
            playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y);
            playerRigidbody.velocity = new Vector2(0f, verticalInput * climbingSpeed);
        }
        else
        {
            // Stop vertical movement when not pressing W or S
            playerRigidbody.velocity = Vector2.zero;
        }

        // Allow horizontal movement to exit the ladder
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            StopClimbing();
        }
    }
}