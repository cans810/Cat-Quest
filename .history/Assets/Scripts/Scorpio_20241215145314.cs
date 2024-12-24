using UnityEngine;

public class Scorpio : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float directionChangeTime = 2f;

    private Rigidbody2D rb;
    private float moveDirection = 1f;
    private float timer;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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

        // Set the "Walk" animation based on the movement
        if (rb.velocity.x != 0)
        {
            animator.SetBool("Walk", true); // Walking animation
        }
        else
        {
            animator.SetBool("Walk", false); // Idle animation
        }
    }

    void ChangeDirection()
    {
        moveDirection = moveDirection == 1f ? -1f : 1f;

        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(moveDirection) * Mathf.Abs(localScale.x);  // Flip the x scale
        transform.localScale = localScale;
    }
}
