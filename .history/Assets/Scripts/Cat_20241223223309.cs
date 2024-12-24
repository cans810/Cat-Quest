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
    public GameObject currentLadder;
    private float ladderXPosition;
    private Vector3 originalScale;
    private bool isTransitioningToClimb = false; // New variable to handle transition

    // Attack state variable
    public CatHitBox hitBox;
    private bool isAttacking = false;

    [Header("Attributes")]
    public float maxHP;
    public float HP;

    public RuntimeAnimatorController currentSkin;
    public bool isAlive;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 2.5f;

        // Physics settings
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        currentSkin = GameManager.Instance.currentCatSkin;
        animator.runtimeAnimatorController = currentSkin;

        isAlive = true;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isAttacking) return;

        if (!isClimbing)
        {
            // Normal movement logic
            moveInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
            }

            // Facing direction
            if (moveInput != 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
                playerFacingDirection = Mathf.Sign(moveInput);
            }

            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetFloat("MoveX", moveInput);

            // Start climbing if near a ladder and press 'E'
            if (currentLadder != null && Input.GetKeyDown(KeyCode.E) && !isClimbing)
            {
                StartClimbing();
            }
        }
        else
        {
            // Reset horizontal movement animation while climbing
            animator.SetFloat("Speed", 0);
            animator.SetFloat("MoveX", 0);
            
            HandleClimbing();

            // Stop climbing if press 'E'
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopClimbing();
            }
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.R) && !isAttacking)
        {
            StartAttack();
        }
    }

    void FixedUpdate()
    {
        if (isAttacking) return;

        if (!isClimbing)
        {
            // Normal movement
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            int contactCount = rb.GetContacts(contacts);
            bool isTouchingWall = false;

            for (int i = 0; i < contactCount; i++)
            {
                if (Mathf.Abs(contacts[i].normal.x) > 0.7f)
                {
                    moveInput = 0;
                    isTouchingWall = true;
                    break;
                }
            }

            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        isTransitioningToClimb = true;

        // Store ladder position
        ladderXPosition = currentLadder.transform.position.x;

        // Reset and freeze everything
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        // Snap to ladder position with a slight lift
        float offsetX = -0.60f;
        transform.position = new Vector2(ladderXPosition + offsetX, transform.position.y + currentLadder.GetComponent<Climbable>().liftOffset);

        // Freeze rotation and horizontal movement
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        // Set a fixed scale while climbing
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Reset transition flag after a short delay
        StartCoroutine(ResetTransitionFlag());
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Vertical movement while climbing
        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            rb.velocity = new Vector2(0f, verticalInput * currentLadder.GetComponent<Climbable>().climbingSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        // Maintain the player at the ladder's X position (but free to move up/down)
        transform.position = new Vector3(ladderXPosition, transform.position.y, transform.position.z);
    }

    private void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 2.5f; // Restore gravity
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Allow rotation again
    }

    private IEnumerator ResetTransitionFlag()
    {
        yield return new WaitForSeconds(0.1f);
        isTransitioningToClimb = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Don't set grounded state if transitioning to climb
        if (!isTransitioningToClimb)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.7f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!isTransitioningToClimb)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tavuk"))
        {
            Chicken chicken = other.GetComponent<Chicken>();
            if (chicken != null)
            {
                HP += chicken.healthValue;
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Ladder"))
        {
            currentLadder = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            if (other.gameObject == currentLadder)
            {
                currentLadder = null;
                if (isClimbing)
                {
                    StopClimbing();
                }
            }
        }
    }

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

    private void ResetAttackState()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack1", false);
        isAttacking = false;
    }

    public void Kill()
    {
        HP = 0;
        gameObject.GetComponent<Animator>().SetBool("dead", true);
        gameObject.GetComponent<Cat>().enabled = false;
        isAlive = false;
    }
}
