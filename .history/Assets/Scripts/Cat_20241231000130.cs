using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 550f;

    [Header("Wall Jumping")]
    public float wallJumpForceX = 600f;     // Horizontal force for wall jumps
    public float wallJumpForceY = 500f;     // Vertical force for wall jumps
    public float wallSlideSpeed = 8f;       // Wall slide speed
    public float wallCheckDistance = 0.6f;   // Distance to check for walls
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    public float playerFacingDirection = 1f;
    public bool isGrounded;
    public bool isClimbing = false;
    public GameObject currentLadder;
    private float ladderXPosition;
    private Vector3 originalScale;
    private bool isTransitioningToClimb = false;
    public CatHitBox hitBox;
    private bool isAttacking = false;
    private bool canWallJump = false;
    private int wallDirection = 0; // 0 = no wall, -1 = left wall, 1 = right wall

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

        moveInput = Input.GetAxisRaw("Horizontal");
        
        // Check for walls
        CheckForWalls();
        
        if (!isClimbing)
        {
            HandleMovementAndJumps();
        }
        else
        {
            HandleClimbing();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isAttacking)
        {
            StartAttack();
        }
    }

    private void CheckForWalls()
    {
        // Check for right wall
        RaycastHit2D rightCheck = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        // Check for left wall
        RaycastHit2D leftCheck = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);

        // Determine wall direction
        if (rightCheck)
        {
            wallDirection = 1;
            canWallJump = true;
        }
        else if (leftCheck)
        {
            wallDirection = -1;
            canWallJump = true;
        }
        else
        {
            wallDirection = 0;
            canWallJump = false;
        }

        // Apply wall slide if touching wall and moving towards it
        if (canWallJump && !isGrounded && ((wallDirection == 1 && moveInput > 0) || (wallDirection == -1 && moveInput < 0)))
        {
            // Wall slide
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
            animator.SetBool("WallSliding", true);
        }
        else
        {
            animator.SetBool("WallSliding", false);
        }
    }

    private void HandleMovementAndJumps()
    {
        // Ground Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }
        // Wall Jump
        else if (Input.GetButtonDown("Jump") && canWallJump && !isGrounded)
        {
            // Only wall jump if moving towards the wall
            if ((wallDirection == 1 && moveInput > 0) || (wallDirection == -1 && moveInput < 0))
            {
                // Jump away from wall
                float jumpDirectionX = -wallDirection * wallJumpForceX;
                rb.velocity = Vector2.zero; // Reset velocity for consistent jumps
                rb.AddForce(new Vector2(jumpDirectionX, wallJumpForceY));
                
                // Flip character
                transform.localScale = new Vector3(-wallDirection * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                playerFacingDirection = -wallDirection;
            }
        }

        // Handle regular movement
        if (moveInput != 0 && !canWallJump)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            playerFacingDirection = Mathf.Sign(moveInput);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetFloat("MoveX", moveInput);
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (!isTransitioningToClimb)
        {
            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                rb.velocity = new Vector2(0f, verticalInput * currentLadder.GetComponent<Climbable>().climbingSpeed);
                animator.SetFloat("Speed", Mathf.Abs(verticalInput));
            }
            else
            {
                rb.velocity = Vector2.zero;
                animator.SetFloat("Speed", 0);
            }
        }

        transform.position = new Vector2(ladderXPosition - 0.3f, transform.position.y);
    }

    void FixedUpdate()
    {
        if (isAttacking) return;

        if (!isClimbing)
        {
            // Only apply horizontal movement if not wall sliding
            if (!canWallJump || isGrounded)
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            }
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        isTransitioningToClimb = true;
        ladderXPosition = currentLadder.transform.position.x;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        transform.position = new Vector2(ladderXPosition - 0.3f, transform.position.y + 0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        StartCoroutine(ResetTransitionFlag());
    }

    private void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 2.5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.localScale = new Vector3(playerFacingDirection * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    private IEnumerator ResetTransitionFlag()
    {
        yield return new WaitForSeconds(0.1f);
        isTransitioningToClimb = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
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
                if (HP + chicken.healthValue <= maxHP)
                {
                    HP += chicken.healthValue;
                }
                else
                {
                    HP = maxHP;
                }
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

    private IEnumerator ResetAttackState()
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

    void OnDrawGizmos()
    {
        // Visualize wall check rays
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.right * wallCheckDistance);
        Gizmos.DrawRay(transform.position, Vector2.left * wallCheckDistance);
    }
}