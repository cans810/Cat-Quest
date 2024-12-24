using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour 
{
    public bool isOnLadder = false;
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f;
    public float liftOffset = 0.1f;
    
    private bool isClimbing = false;
    private float ladderXPosition;
    private Cat catScript; // Reference to the cat script
    private Vector3 originalPlayerScale;
    
    void Update()
    {
        if (isOnLadder && Input.GetKeyDown(KeyCode.E) && !isClimbing)
        {
            StartClimbing();
        }
        else if (isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            StopClimbing();
        }

        if (isClimbing)
        {
            HandleClimbing();
        }
    }

    private void StartClimbing()
    {
        if (playerRigidbody != null && catScript != null)
        {
            isClimbing = true;

            // Disable gravity and stop movement
            playerRigidbody.gravityScale = 0;
            playerRigidbody.velocity = Vector2.zero;

            // Lock rotation and position
            playerRigidbody.freezeRotation = true;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

            // Align to ladder
            playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y + liftOffset);

            // Fix the scale to prevent flipping
            catScript.transform.localScale = new Vector3(
                Mathf.Abs(originalPlayerScale.x),
                originalPlayerScale.y,
                originalPlayerScale.z
            );
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = true;
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
            catScript = collision.GetComponent<Cat>();
            originalPlayerScale = collision.transform.localScale;
            ladderXPosition = transform.position.x;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOnLadder = false;

            // Clear references
            playerRigidbody = null;
            catScript = null;
        }
    }
    
    private void HandleClimbing()
    {
        if (playerRigidbody != null)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            // Vertical movement
            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                playerRigidbody.velocity = new Vector2(0f, verticalInput * climbingSpeed);
            }
            else
            {
                // Complete stop when no input
                playerRigidbody.velocity = Vector2.zero;
            }
            
            // Keep centered on ladder
            playerRigidbody.position = new Vector2(ladderXPosition, playerRigidbody.position.y);
        }
    }
}