using UnityEngine;
using UnityEngine.Tilemaps;

public class Cat : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 550f;
    public float smoothing = 0.05f; // Hareket yumuşatması için yeni değişken
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    public bool isGrounded;
    private bool canWallJump;
    private float wallJumpTimer = 0f;
    private float wallJumpDuration = 0.2f;
    private Vector2 currentVelocity;

    public bool isClimbing = false;
    private TilemapCollider2D ladderTilemapCollider;
    private Tilemap ladderTilemap;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 2.5f;

        // Fizik ayarları
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    
    void Update()
    {
        if (!isClimbing)
        {
            // Hareket girişini alma
            float targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
            
            // Duvar zıplama kontrolü
            if (wallJumpTimer > 0)
            {
                wallJumpTimer -= Time.deltaTime;
                if (wallJumpTimer <= 0)
                {
                    canWallJump = false;
                }
            }
            
            // Zıplama kontrolü
            if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || canWallJump))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
                canWallJump = false;
            }
            
            // Animator güncelleme
            animator.SetFloat("Speed", Mathf.Abs(targetSpeed));
            animator.SetFloat("MoveX", targetSpeed);
            
            // Karakter yönü
            if (targetSpeed != 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Sign(targetSpeed) * Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
            }
        }
        
        // Tırmanış animasyonları
        if (isClimbing)
        {
            animator.SetFloat("ClimbingSpeed", Mathf.Abs(Input.GetAxisRaw("Vertical")));
            // Center the cat on the ladder tile
            if (ladderTilemap != null)
            {
                Vector3 catWorldPos = transform.position;
                Vector3Int tilePos = ladderTilemap.WorldToCell(catWorldPos);
                Vector3 tileCenter = ladderTilemap.CellToWorld(tilePos) + ladderTilemap.cellSize / 2;

                // Position the cat slightly to the left of the center of the tile
                float offsetX = -0.15f;  // Adjust this value to move the cat to the left or right
                transform.position = new Vector3(tileCenter.x + offsetX, transform.position.y, transform.position.z); // Adjust x position slightly to the left
            }
        }
    }
    
    void FixedUpdate()
    {
        if (!isClimbing)
        {
            // Yumuşak hareket hesaplama
            float targetSpeedX = Input.GetAxisRaw("Horizontal") * speed;
            float smoothedSpeedX = Mathf.SmoothDamp(
                rb.velocity.x,
                targetSpeedX,
                ref currentVelocity.x,
                smoothing
            );
            
            // Velocity güncelleme
            rb.velocity = new Vector2(smoothedSpeedX, rb.velocity.y);
        }
        else
        {
            // Tırmanış sırasında hareket
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(0, vertical * speed);
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Yerde olma kontrolü
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                break;
            }
            // Duvar kontrolü
            else if (Mathf.Abs(contact.normal.x) > 0.7f && !isGrounded)
            {
                canWallJump = true;
                wallJumpTimer = wallJumpDuration;
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
        
        // When the cat starts interacting with a ladder (TilemapCollider2D)
        if (other.CompareTag("Ladder"))
        {
            ladderTilemapCollider = other.GetComponent<TilemapCollider2D>();
            ladderTilemap = other.GetComponentInParent<Tilemap>(); // Make sure to get the Tilemap that the collider belongs to
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the cat leaves the ladder
        if (other.CompareTag("Ladder"))
        {
            ladderTilemapCollider = null;
            ladderTilemap = null;
        }
    }
}
