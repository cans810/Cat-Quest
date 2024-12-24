using UnityEngine;

public class Climbable : MonoBehaviour
{
    public string ladderTag = "Ladder";
    public float liftOffset = 1f;  // Lift offset to slightly elevate the player when climbing
    public float climbingSpeed = 5f; // Speed at which the player climbs

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
            Cat cat = other.GetComponent<Cat>();
            if (cat != null && !cat.isClimbing)
            {
                // Notify the Cat script about the ladder collision
                cat.currentLadder = this.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cat cat = other.GetComponent<Cat>();
            if (cat != null)
            {
                // Remove the ladder reference when exiting the ladder trigger
                cat.currentLadder = null;
            }
        }
    }
}
