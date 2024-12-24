using UnityEngine;
using System.Collections;

public class EnemyBasic : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float directionChangeTime = 2f;
    public float runDuration = 2f; // Time for running in the opposite direction

    private Rigidbody2D rb;
    private float moveDirection = 1f;
    private float timer;

    private Animator animator;

    private bool isRunningOpposite = false; // To check if the enemy is running in the opposite direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        timer = directionChangeTime;
    }

    void Update()
    {
        if (!isRunningOpposite)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ChangeDirection();
                timer = directionChangeTime;
            }

            Move();
        }
        else
        {
            // If the enemy is running in the opposite direction, do that for 'runDuration'
            Move();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (rb.velocity.x != 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false); 
        }
    }

    void ChangeDirection()
    {
        moveDirection = moveDirection == 1f ? -1f : 1f;

        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(moveDirection) * Mathf.Abs(localScale.x); 
        transform.localScale = localScale;
    }

    // Method to make the enemy run in the opposite direction for a limited time
    public void RunInOppositeDirection()
    {
        if (!isRunningOpposite) // To prevent multiple calls
        {
            isRunningOpposite = true;
            StartCoroutine(RunInOppositeDirectionCoroutine());
        }
    }

    // Coroutine to handle the running in the opposite direction
    private IEnumerator RunInOppositeDirectionCoroutine()
    {
        moveDirection = -moveDirection; // Reverse the direction
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(moveDirection) * Mathf.Abs(localScale.x); 
        transform.localScale = localScale;

        // Wait for the specified run duration before stopping
        yield return new WaitForSeconds(runDuration);

        StopRunningInOppositeDirection();
    }

    private void StopRunningInOppositeDirection()
    {
        isRunningOpposite = false;
    }
}
