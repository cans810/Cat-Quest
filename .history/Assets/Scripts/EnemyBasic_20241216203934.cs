using UnityEngine;
using System.Collections;

public class EnemyBasic : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float directionChangeTime = 2f;
    public float runDuration = 2f;
    public float runSpeedMultiplier = 1.5f;

    private Rigidbody2D rb;
    private float moveDirection = 1f;
    private float timer;
    private Animator animator;
    private bool isRunningOpposite = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = directionChangeTime;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Cat>().Kill();
        }
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

    public void RunInOppositeDirection(float playerFacingDirection)
    {
        if (!isRunningOpposite)
        {
            isRunningOpposite = true;
            StartCoroutine(RunInOppositeDirectionCoroutine(playerFacingDirection));
        }
    }

    private IEnumerator RunInOppositeDirectionCoroutine(float playerFacingDirection)
    {
        moveDirection = Mathf.Sign(playerFacingDirection) * 1; 
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(moveDirection) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;

        float originalSpeed = moveSpeed;
        moveSpeed *= runSpeedMultiplier;

        yield return new WaitForSeconds(runDuration);

        moveSpeed = originalSpeed;
        StopRunningInOppositeDirection();
    }

    private void StopRunningInOppositeDirection()
    {
        isRunningOpposite = false;
    }
}
