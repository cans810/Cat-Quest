using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("Climbing")]
    public float climbSpeed = 5f;

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
    public bool isJumpRequested;
    public bool isGrounded;
    public float groundedRememberTime = 0.2f;
    public float groundedRememberTimeCounter;

    // Climbing variables
    public bool isClimbing = false;
    private TilemapCollider2D ladderCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Enhanced physics settings
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 2.5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (!isClimbing)
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
            animator.SetFloat("MoveX", moveInput);
            animator.SetBool("IsGrounded", isGrounded);
        }
        else
        {
            // Climbing mechanics
            float vertical = Input.GetAxisRaw("Vertical");
            
            // Ladder positioning
            if (ladderCollider != null)
            {
                Vector2 colliderCenter = ladderCollider.bounds.center;
                float offsetX = -0.30f;  
                transform.position = new Vector2(colliderCenter.x + offsetX, transform.position.y); 
            }
        }
    }

    void FixedUpdate()
    {
        if (!isClimbing)
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
        else
        {
            // Climbing movement
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(0, vertical * climbSpeed);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tavuk"))
        {
            var barkın = other.GetComponent<tavuk>();
            if (barkın != null)
            {
                FindObjectOfType<puanMANAGER>().AddScore(10);
                Destroy(other.gameObject);
            }
        }
        
        // When the cat starts interacting with a ladder
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            ladderCollider = other.GetComponent<TilemapCollider2D>();
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the cat leaves the ladder
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            ladderCollider = null;
            rb.gravityScale = 2.5f;
        }
    }

    // Optional: Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}