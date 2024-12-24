using UnityEngine;

public class Climbable : MonoBehaviour
{
    public string ladderTag = "Ladder";
    public float climbingSpeed = 10f;
    public float horizontalOffset = 0.2f;

    void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("Climbable object must have a Collider2D with 'Is Trigger' enabled.");
        }
    }
}