using UnityEngine;

public class Climbable : MonoBehaviour
{
    public string ladderTag = "Ladder";

    void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("Climbable object must have a Collider2D with 'Is Trigger' enabled.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Cat>().isClimbing = true; // Enable climbing on player
            other.GetComponent<Rigidbody2D>().gravityScale = 0f; // Disable gravity during climbing
        }
    }

    // Trigger when player exits the ladder area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // When the player exits the ladder
        {
            other.GetComponent<Cat>().isClimbing = false; // Disable climbing
            other.GetComponent<Rigidbody2D>().gravityScale = 2.5f; // Re-enable gravity
        }
    }
}
