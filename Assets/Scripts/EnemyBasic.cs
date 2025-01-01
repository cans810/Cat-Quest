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
    private bool canAttack = true; // Tracks cooldown status
    private bool isAttacking = false; // Tracks attack state

    public GameObject hitbox;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = directionChangeTime;

        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform child in childTransforms)
        {
            if (child.name == "EnemyHitbox")
            {
                hitbox = child.gameObject;
                break;
            }
        }

        if (hitbox == null)
        {
            Debug.LogWarning("EnemyHitbox not found in child components for " + gameObject.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Cat>().Kill();
        }
        else
        {
            ChangeDirection(); // Change direction if collided with something other than the player
        }
    }

    void Update()
    {
        if (gameObject.name.Equals("Snake"))
        {
            if (hitbox != null)
            {
                foreach (GameObject objectinhitbox in hitbox.GetComponent<EnemyHitbox>().GetObjectsInHitbox())
                {
                    if (objectinhitbox.tag.Equals("Player"))
                    {
                        AttackAnim(objectinhitbox);
                        return; // Skip movement during attack
                    }
                }
            }
        }

        if (!isRunningOpposite && !isAttacking)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ChangeDirection();
                timer = directionChangeTime;
            }

            Move();
        }
        else if (!isAttacking)
        {
            Move();
        }
    }

    void Move()
    {
        if (isAttacking)
        {
            rb.velocity = Vector2.zero; // Stop movement during attack
            animator.SetBool("Walk", false);
            return;
        }

        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        animator.SetBool("Walk", rb.velocity.x != 0);
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

    public void AttackAnim(GameObject player)
    {
        if (canAttack)
        {
            StartCoroutine(AttackWithCooldown(player));
        }
    }

    private IEnumerator AttackWithCooldown(GameObject player)
    {
        isAttacking = true; // Enter attack state
        canAttack = false;

        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(2f);

        canAttack = true;
        isAttacking = false; // Exit attack state
        animator.SetBool("Attack", false);
    }

    public void DamagePlayer(GameObject player){
        if (player != null && player.GetComponent<Cat>() != null)
        {
            player.GetComponent<Cat>().Kill();
        }
    }
}
