using UnityEngine;

public class Cat : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float acceleration = 50f;
    public float deceleration = 50f;
    public float velocityPower = 0.96f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.2f;
    public float jumpCutMultiplier = 0.5f;

    [Header("Air Control")]
    public float airMoveMultiplier = 0.5f;
    public float airDragMultiplier = 0.95f;

    [Header("Collision")]
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    // Movement variables
    private float moveInput;
    private bool isFacingRight = true;
    private float coyoteTimeCounter;

    // Jump variables
    private bool isJumpRequested;
    private bool isGrounded;
    private float groundedRememberTime = 0.2f;
    private float groundedRememberTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Enhanced physics settings
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

        // Coyote time management
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            groundedRememberTimeCounter = groundedRememberTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            groundedRememberTimeCounter -= Time.deltaTime;
        }

        // Get movement input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip character
        if (moveInput != 0)
        {
            Flip(moveInput);
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && groundedRememberTimeCounter > 0f)
        {
            isJumpRequested = true;
        }

        // Jump cut
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        // Update animator
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        // Movement
        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        // Calculates the movement based on Dead Cells-like acceleration
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPower) * Mathf.Sign(speedDiff);

        // Apply different multipliers for ground and air
        if (!isGrounded)
        {
            movement *= airMoveMultiplier;
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        rb.AddForce(movement * Vector2.right);

        // Jump
        if (isJumpRequested && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumpRequested = false;
            coyoteTimeCounter = 0;
        }
    }

    void Flip(float moveInput)
    {
        if (moveInput > 0 && !isFacingRight || moveInput < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    // Optional: Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}