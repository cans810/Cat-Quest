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
    public float wallSlideSpeed = 3f; // Dead Cells-like sliding speed
    public float wallJumpCooldown = 0.2f; // Cooldown for wall jump
    public LayerMask wallLayer;

    private bool isTouchingWall = false;
    private bool isWallSliding = false;
    private float lastWallJumpTime = 0f;
    private float wallDirectionX;

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

        CheckForWalls();

        if (!isWallSliding)
        {
            HandleNormalMovement();
        }

        HandleWallJump();

        if (Input.GetKeyDown(KeyCode.R) && !isAttacking)
        {
            StartAttack();
        }
    }

    private void CheckForWalls()
    {
        Vector2 wallCheckPosition = new Vector2(transform.position.x + playerFacingDirection * 0.5f, transform.position.y);
        isTouchingWall = Physics2D.OverlapCircle(wallCheckPosition, 0.2f, wallLayer);

        if (isTouchingWall && !isGrounded && Time.time - lastWallJumpTime > wallJumpCooldown)
        {
            StartWallSliding();
        }
        else
        {
            StopWallSliding();
        }
    }

    private void StartWallSliding()
    {
        if (isWallSliding) return;

        isWallSliding = true;
        rb.velocity = new Vector2(0, -wallSlideSpeed);
        animator.SetBool("WallSliding", true);
    }

    private void StopWallSliding()
    {
        if (!isWallSliding) return;

        isWallSliding = false;
        animator.SetBool("WallSliding", false);
    }

    private void HandleWallJump()
    {
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            lastWallJumpTime = Time.time;

            Vector2 wallJumpDirection = new Vector2(-playerFacingDirection, 1f).normalized;

            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(wallJumpDirection.x * wallJumpForce, wallJumpDirection.y * wallJumpUpForce), ForceMode2D.Impulse);

            transform.localScale = new Vector3(-playerFacingDirection * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            playerFacingDirection = -playerFacingDirection;

            StopWallSliding();
        }
    }

    void FixedUpdate()
    {
        if (isAttacking || isWallSliding) return;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleNormalMovement()
    {
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
    }

    private void OnCollisionStay2D(Collision2D collision)
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
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
        animator.SetBool("Dead", true);
        this.enabled = false;
        isAlive = false;
    }
}
