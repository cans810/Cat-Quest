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
            other.GetComponent<Cat>().isClimbing = true; 
            other.GetComponent<Rigidbody2D>().gravityScale = 0f; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            other.GetComponent<Cat>().isClimbing = false; // Disable climbing
            other.GetComponent<Rigidbody2D>().gravityScale = 2.5f; // Re-enable gravity
        }
    }
}
