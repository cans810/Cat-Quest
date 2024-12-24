using UnityEngine;
using UnityEngine.Tilemaps;

public class Cat : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 550f;
    
    private Rigidbody2D rb;
    private Animator animator;

    // Movement variables
    private float moveInput;
    private bool isFacingRight = true;

    // Ground and climbing variables
    public bool isGrounded;
    public bool isClimbing = false;
    private TilemapCollider2D ladderCollider;
    
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
        if (!isClimbing)
        {
            // Movement input
            moveInput = Input.GetAxisRaw("Horizontal");
            
            // Jump
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
            }
            
            // Animator updates
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetFloat("MoveX", moveInput);
        }
        
        // Climbing mechanics
        if (isClimbing)
{
    if (ladderCollider != null)
    {
        Vector2 colliderCenter = ladderCollider.bounds.center;
        float offsetX = transform.localScale.x > 0 ? -0.30f : 0.30f;
        transform.position = new Vector2(colliderCenter.x + offsetX, transform.position.y);
    }
}
    }
    
    void FixedUpdate()
    {
        if (!isClimbing)
        {
            // Simple Mario-like horizontal movement
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Climbing movement
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(0, vertical * moveSpeed);
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Ground check
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
            tavuk barkın = other.GetComponent<tavuk>();
            if (barkın != null)
            {
                FindObjectOfType<puanMANAGER>().AddScore(10);
                Destroy(other.gameObject);
            }
        }
        
        // Ladder interaction
        if (other.CompareTag("Ladder"))
        {
            ladderCollider = other.GetComponent<TilemapCollider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Leaving ladder
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            ladderCollider = null;
            rb.gravityScale = 2.5f;
        }
    }
}