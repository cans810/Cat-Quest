using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false; 
    public Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f; 
    public float climbingAcceleration = 0.1f; 
    public BoxCollider2D playerCollider;

    private float currentClimbingVelocity = 0f;
    private float inputVertical;

    void Update()
    {
        if (isOnLadder && Input.GetKeyDown(KeyCode.E))
        {
            ToggleClimbing();
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
            playerCollider = collision.GetComponent<BoxCollider2D>();
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
        if (playerRigidbody != null && playerCollider != null)
        {
            var cat = playerRigidbody.gameObject.GetComponent<Cat>();
            cat.isClimbing = true;
            playerRigidbody.gravityScale = 0; 
            playerRigidbody.velocity = Vector2.zero; 
            playerCollider.enabled = false;
        }
    }

    private void StopClimbing()
    {
        if (playerRigidbody != null && playerCollider != null)
        {
            var cat = playerRigidbody.gameObject.GetComponent<Cat>();
            if (cat.isClimbing)
            {
                cat.isClimbing = false;
                playerRigidbody.gravityScale = 2.5f; 
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f);
                playerCollider.enabled = true; 
            }
        }
    }

    private void HandleClimbing()
    {
        inputVertical = Input.GetAxisRaw("Vertical");

        // Smoothly adjust climbing velocity
        currentClimbingVelocity = Mathf.Lerp(currentClimbingVelocity, inputVertical * climbingSpeed, climbingAcceleration);

        // Apply climbing movement
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, currentClimbingVelocity);
    }
}
