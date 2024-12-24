using UnityEngine;

public class Climbable : MonoBehaviour
{
    public string ladderTag = "Ladder";
    public float liftOffset = 1f;  // Slight elevation for the player when climbing
    public float climbingSpeed = 5f; // Speed for climbing movement

    void Start()
    {
        if (GetComponent<Collider2D>() == null || !GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogError("Climbable object must have a Collider2D with 'Is Trigger' enabled.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cat cat = other.GetComponent<Cat>();
            if (cat != null)
            {
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
                if (cat.isClimbing) cat.StopClimbing(); // Ensure climbing stops when exiting
                cat.currentLadder = null;
            }
        }
    }
}
