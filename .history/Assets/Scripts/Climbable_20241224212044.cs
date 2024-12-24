using UnityEngine;

public class Climbable : MonoBehaviour
{
    public string ladderTag = "Ladder";
    public float climbingSpeed = 50f;

    void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("Climbable object must have a Collider2D with 'Is Trigger' enabled.");
        }
    }
}