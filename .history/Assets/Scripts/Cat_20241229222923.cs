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

    [Header("Attributes")]
    public float maxHP;
    public float HP;
    public RuntimeAnimatorController currentSkin;
    public bool isAlive;

    [Header("Wall Jumping")]
    public float wallJumpForce = 400f;
    public float wallJumpUpForce = 550f;
    public float wallSlideSpeed = 4f;  // Increased for faster sliding
    public float wallJumpTime = 0.15f; // Time window to perform wall jump after leaving wall
    public float wallJumpControl = 0.5f; // How much control player has during wall jump
    public LayerMask wallLayer;

    private bool isTouchingWall = false;
    private bool isWallSliding = false;
    private float wallDirectionX;
    private float lastWallTouchTime;
    private bool canWallJump => Time.time - lastWallTouchTime <= wallJumpTime;

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

        CheckForWalls(); // Check for walls

        if (!isClimbing)
        {
            HandleNormalMovement();
        }
        else
        {
            HandleClimbing();
        }

        HandleWallJump(); // Handle wall jump

        if (Input.GetKeyDown(KeyCode.R) && !isAttacking)
        {
            StartAttack();
        }
    }

    private void CheckForWalls()
{
    Vector2 wallCheckPosition = new Vector2(transform.position.x + playerFacingDirection * 0.5f, transform.position.y);
    isTouchingWall = Physics2D.OverlapCircle(wallCheckPosition, 0.2f, wallLayer);

    // Allow sticking only if cooldown has passed
    if (isTouchingWall && Time.time - lastWallJumpTime > wallStickCooldown)
    {
        if (!isGrounded)
        {
            StartWallSliding();
        }
    }
    else
    {
        StopWallSliding();
    }
}

    private void StartWallSliding()
    {
        if (isWallSliding) return; // Already wall sliding

        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        animator.SetBool("WallSliding", true);
    }

    private void StopWallSliding()
    {
        if (!isWallSliding) return; // Not wall sliding

        isWallSliding = false;
        animator.SetBool("WallSliding", false);
    }

    private void HandleWallJump()
    {
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            // Reset velocity for consistent jump
            rb.velocity = Vector2.zero;

            // Wall jump direction is opposite to the wall
            Vector2 wallJumpDirection = new Vector2(-playerFacingDirection, 1f).normalized;

            // Apply forces for the jump
            rb.AddForce(new Vector2(wallJumpDirection.x * wallJumpForce, wallJumpDirection.y * wallJumpUpForce), ForceMode2D.Impulse);

            // Prevent immediate re-sticking
            lastWallJumpTime = Time.time;

            // Flip the character to face away from the wall
            transform.localScale = new Vector3(-wallDirectionX * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            playerFacingDirection = -playerFacingDirection;

            StopWallSliding(); // Exit sliding state
        }
    }
    void FixedUpdate()
    {
        if (isAttacking) return;

        if (!isClimbing && !isWallSliding)
        {
            // Normal movement
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else if (isWallSliding)
        {
            // Apply more control during wall slides
            float targetXVelocity = moveInput * moveSpeed * wallJumpControl;
            rb.velocity = new Vector2(
                Mathf.Lerp(rb.velocity.x, targetXVelocity, Time.fixedDeltaTime * 10f),
                rb.velocity.y
            );
        }
    }

    private void HandleNormalMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            playerFacingDirection = Mathf.Sign(moveInput);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetFloat("MoveX", moveInput);
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

    private void StopWallSliding()
    {
        isWallSliding = false;
        animator.SetBool("WallSliding", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tavuk"))
        {
            Chicken chicken = other.GetComponent<Chicken>();
            if (chicken != null)
            {
                if (HP + chicken.healthValue <= maxHP){
                    HP += chicken.healthValue;
                }
                else{
                    HP += maxHP;
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
}