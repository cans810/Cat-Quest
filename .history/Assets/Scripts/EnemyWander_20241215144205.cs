using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;    // How fast the enemy moves
    public float directionChangeTime = 2f;  // Time before changing direction

    private Rigidbody2D rb;
    private float moveDirection = 1f;  // 1 for right, -1 for left
    private float timer;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Set the initial direction change timer
        timer = directionChangeTime;
    }

    void Update()
    {
        // Countdown the timer and change direction when it reaches 0
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ChangeDirection();
            timer = directionChangeTime;
        }

        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void ChangeDirection()
    {
        // Reverse the movement direction (opposite direction)
        moveDirection = moveDirection == 1f ? -1f : 1f;

        // Flip the enemy's sprite to face the new direction (optional)
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(moveDirection) * Mathf.Abs(localScale.x);  // Flip the x scale
        transform.localScale = localScale;
    }
}
