using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 550f;

    private Rigidbody2D rb;
    private Animator animator;

    // Movement and climbing variables
    private float moveInput;
    public bool isGrounded;
    public bool isClimbing = false;
    public GameObject currentLadder;
    private float ladderXPosition;

    // Attributes
    public float maxHP;
    public float HP;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 2.5f;
    }

    void Update()
    {
        if (!isClimbing)
        {
            HandleMovement();
        }
        else
        {
            HandleClimbing();
        }

        if (Input.GetKeyDown(KeyCode.E) && currentLadder != null)
        {
            if (!isClimbing) StartClimbing();
            else StopClimbing();
        }
    }

    void FixedUpdate()
    {
        if (!isClimbing)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    private void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }

        // Flip character direction
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        ladderXPosition = currentLadder.transform.position.x;

        // Snap position and disable gravity
        transform.position = new Vector2(ladderXPosition, transform.position.y);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        animator.SetBool("IsClimbing", true);
    }

    private void HandleClimbing()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            rb.velocity = new Vector2(0f, verticalInput * currentLadder.GetComponent<Climbable>().climbingSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        // Maintain X position on the ladder
        transform.position = new Vector3(ladderXPosition, transform.position.y, transform.position.z);
    }

    public void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 2.5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        animator.SetBool("IsClimbing", false);
    }

    void OnCollisionStay2D(Collision2D collision)
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

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
