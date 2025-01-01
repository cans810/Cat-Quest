using UnityEngine;

public class Cat : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float wallJumpForceX = 5f;
    public float wallJumpForceY = 10f;
    public float wallSlideSpeed = 2f;
    public float wallDetectionRadius = 0.2f;

    [Header("References")]
    public LayerMask wallLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded;
    private bool isTouchingWall;
    private int wallSide; // 1 for right, -1 for left, 0 for none
    private float moveInput;

    private Vector3 originalScale;
    private int playerFacingDirection = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        HandleInput();
        CheckGrounded();
        CheckWalls();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip character based on movement direction
        if (moveInput != 0)
        {
            playerFacingDirection = (int)Mathf.Sign(moveInput);
            transform.localScale = new Vector3(playerFacingDirection * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Handle Jump Input
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (isTouchingWall)
            {
                WallJump();
            }
        }
    }

    private void HandleMovement()
    {
        if (isTouchingWall && !isGrounded)
        {
            // Prevent horizontal movement while wall sliding
            rb.velocity = new Vector2(0, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            // Normal horizontal movement
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void WallJump()
    {
        rb.velocity = Vector2.zero; // Reset velocity
        rb.AddForce(new Vector2(-wallSide * wallJumpForceX, wallJumpForceY), ForceMode2D.Impulse);

        // Flip character to face away from the wall
        transform.localScale = new Vector3(-wallSide * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        playerFacingDirection = -wallSide;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void CheckWalls()
    {
        Vector2 rightCheck = (Vector2)transform.position + Vector2.right * wallDetectionRadius;
        Vector2 leftCheck = (Vector2)transform.position + Vector2.left * wallDetectionRadius;

        bool touchingRight = Physics2D.OverlapCircle(rightCheck, wallDetectionRadius, wallLayer);
        bool touchingLeft = Physics2D.OverlapCircle(leftCheck, wallDetectionRadius, wallLayer);

        isTouchingWall = touchingLeft || touchingRight;
        wallSide = touchingRight ? 1 : touchingLeft ? -1 : 0;

        animator.SetBool("WallSliding", isTouchingWall && !isGrounded);
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    private void OnDrawGizmos()
    {
        // Ground Check Gizmo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        // Wall Check Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.right * wallDetectionRadius, wallDetectionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.left * wallDetectionRadius, wallDetectionRadius);
    }
}
