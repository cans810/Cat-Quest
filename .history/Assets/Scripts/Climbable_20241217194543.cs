using UnityEngine;

public class Climbable : MonoBehaviour
{
    public bool isOnLadder = false; // Player is in the ladder area
    private Rigidbody2D playerRigidbody;
    public float climbingSpeed = 4f; // Speed for climbing

    private void Update()
    {
        if (isOnLadder && playerRigidbody != null)
        {
            HandleClimbing();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 0f; // Disable gravity when on ladder
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 2.5f; // Restore gravity
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f); // Stop climbing movement
                playerRigidbody = null;
            }
        }
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxisRaw("Vertical"); // Get W/S or Up/Down input
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, verticalInput * climbingSpeed); // Apply vertical movement
    }
}
