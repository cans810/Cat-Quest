using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;    
    public float directionChangeTime = 2f; 

    private Rigidbody2D rb;
    private float moveDirection = 1f;      
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timer = directionChangeTime;
    }

    void Update()
    {
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
        // Set the velocity to move the enemy left or right
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void ChangeDirection()
    {
        // Change the movement direction
        moveDirection = -moveDirection;

        // Flip the enemy's sprite to face the new direction (optional)
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(moveDirection) * Mathf.Abs(localScale.x);  // Flip the x scale
        transform.localScale = localScale;
    }
}
