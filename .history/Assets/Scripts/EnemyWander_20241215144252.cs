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
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    void ChangeDirection()
    {
        moveDirection = moveDirection == 1f ? 1f : -1f;

        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(moveDirection) * Mathf.Abs(localScale.x); 
        transform.localScale = localScale;
    }
}
