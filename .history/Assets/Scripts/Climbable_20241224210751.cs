using UnityEngine;

public class Climbable : MonoBehaviour
{
    public string ladderTag = "Ladder";
    public float climbingSpeed = 5f;
    public float horizontalOffset = -0.6f;

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
            if (cat != null)
            {
                cat.currentLadder = gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cat cat = other.GetComponent<Cat>();
            if (cat != null && cat.currentLadder == gameObject)
            {
                cat.currentLadder = null;
            }
        }
    }
}