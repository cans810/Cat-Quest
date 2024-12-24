using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 550f;

    private Rigidbody2D rb;
    private Animator animator;

    // Movement variables
    private float moveInput;
    public float playerFacingDirection = 1f;

    // Ground and climbing variables
    public bool isGrounded;
    public bool isClimbing = false;
    private GameObject currentLadder;

    // Attack state variable
    public CatHitBox hitBox;
    private bool isAttacking = false;

    [Header("Attributes")]
    public float maxHP;
    public float HP;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 2.5f;

        // Physics settings
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        if (isAttacking) return; // Prevent movement and actions during attack

        if (!isClimbing)
        {
            // Horizontal movement input
            moveInput = Input.GetAxisRaw("Horizontal");

            // Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
            }

            // Facing direction (for sprite flipping)
            if (moveInput != 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );

                playerFacingDirection = Mathf.Sign(moveInput);
            }

            // Animator updates for movement
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetFloat("MoveX", moveInput);

            // Attack input
            if (Input.GetKeyDown(KeyCode.R) && !isAttacking) // Check for "R" key press
            {
                StartAttack();
            }
        }

        // Climbing mechanics
        if (isClimbing && currentLadder != null)
        {
            // Climbing movement (up and down)
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(0, vertical * moveSpeed);

            // Snap to the ladder X position (prevent horizontal movement off the ladder)
            transform.position = new Vector2(currentLadder.transform.position.x, transform.position.y);
        }
    }

    void FixedUpdate()
    {
        if (isAttacking || isClimbing) return; // Prevent movement during attack and climbing

        if (!isClimbing)
        {
            // Apply horizontal movement when not climbing
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Ground check (if player is on the ground)
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tavuk"))
        {
            Chicken chicken = other.GetComponent<Chicken>();
            if (chicken != null)
            {
                Destroy(other.gameObject);
            }
        }

        // Ladder interaction
        if (other.CompareTag("Ladder"))
        {
            GameObject ladder = other.gameObject;
            if (ladder != null)
            {
                // Player enters ladder area
                isClimbing = true;
                currentLadder = ladder;
                rb.gravityScale = 0f; // Disable gravity while climbing
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            GameObject ladder = other.gameObject;
            if (ladder == currentLadder)
            {
                // Player exits ladder area
                isClimbing = false;
                currentLadder = null;
                rb.gravityScale = 2.5f; // Re-enable gravity
            }
        }
    }

    // Start attack animation
    private void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("Attack1", true);

        if (hitBox.objectsInHitbox.Count > 0)
        {
            foreach (GameObject enemy in hitBox.objectsInHitbox)
            {
                enemy.GetComponent<EnemyBasic>().RunInOppositeDirection(playerFacingDirection);
            }
        }

        StartCoroutine(ResetAttackState());
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(1f);

        animator.SetBool("Attack1", false);
        isAttacking = false;
    }

    public void Kill()
    {
        gameObject.GetComponent<Animator>().SetBool("dead", true);
        gameObject.GetComponent<Cat>().enabled = false;
    }
}
