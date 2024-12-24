using UnityEngine;

public class Climbable : MonoBehaviour
{
    public string ladderTag = "Ladder";
    public float climbingSpeed = 5f;
    public float horizontalOffset = 0.6f;

    void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("Climbable object must have a Collider2D with 'Is Trigger' enabled.");
        }
    }
}