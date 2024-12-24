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
        // Only allow toggling climbing when not already climbing
        if (isOnLadder && Input.GetKeyDown(KeyCode.E) && !isClimbing)
        {
            StartClimbing();
        }
        // Only allow exiting when not moving and pressing E
        else if (isClimbing && Input.GetKeyDown(KeyCode.E) && playerRigidbody.velocity.magnitude < 0.1f)
        {
            StopClimbing();
        }

        // Handle climbing movement
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
            playerCollider.enabled = false;
            playerRigidbody.position += new Vector2(0, liftOffset);
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null && playerCollider != null)
        {
            isClimbing = false;
            playerRigidbody.gravityScale = 2.5f;
            playerCollider.enabled = true;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f);
        }
    }

    private void HandleClimbing()
    {
        float verticalInput = 0f;
        
        // Use W and S keys for climbing
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        // Move only when W or S is pressed
        if (verticalInput != 0f)
        {
            playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y);
            playerRigidbody.velocity = new Vector2(0f, verticalInput * climbingSpeed);
        }
        else
        {
            playerRigidbody.velocity = Vector2.zero;
        }
    }
}