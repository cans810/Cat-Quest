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
    private GameObject currentLadder;

    // Attack state variable
    public CatHitBox hitBox;
    private bool isAttacking = false;

    [Header("Attributes")]
    public float maxHP;
    public float HP;

    public RuntimeAnimatorController currentSkin;


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
    }

    void Update()
    {
        if (isAttacking) return; // Prevent movement and actions during attack

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

                //animator.SetBool("IsJumping", true);
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

            // Animator updates for movement
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetFloat("MoveX", moveInput);

            // Attack input
            if (Input.GetKeyDown(KeyCode.R) && !isAttacking) // Check for "R" key press
            {
                StartAttack();
            }
        }

        // Climbing mechanics
        if (isClimbing && currentLadder != null)
        {
            // Snap X position only when starting to climb
            if (Mathf.Abs(transform.position.x - currentLadder.transform.position.x) > 0.1f)
            {
                Vector2 ladderPosition = currentLadder.transform.position;
                float offsetX = transform.localScale.x > 0 ? -0.30f : 0.30f;
                transform.position = new Vector2(ladderPosition.x + offsetX, transform.position.y);
            }
        }

    }

    void FixedUpdate()
    {
        if (isAttacking) return; // Prevent movement during attack

        if (!isClimbing)
        {
            // Check if the player is touching a wall
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            int contactCount = rb.GetContacts(contacts);
            bool isTouchingWall = false;

            for (int i = 0; i < contactCount; i++)
            {
                // If wall contact is detected and player is trying to move into the wall
                if (Mathf.Abs(contacts[i].normal.x) > 0.7f)
                {
                    // Stop horizontal movement when touching a wall
                    moveInput = 0;
                    isTouchingWall = true;
                    break;
                }
            }

            // Apply movement only if not touching a wall
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, vertical * moveSpeed);
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
                //animator.SetBool("IsJumping", false);
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
            Chicken chicken = other.GetComponent<Chicken>();
            if (chicken != null)
            {
                HP += chicken.healthValue;
                Destroy(other.gameObject);
            }
        }

        // Ladder interaction
        if (other.CompareTag("Ladder"))
        {
            GameObject ladder = other.gameObject;
            if (ladder != null)
            {
                if (currentLadder == null)
                {
                    currentLadder = ladder;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            GameObject ladder = other.gameObject;
            if (ladder == currentLadder)
            {
                isClimbing = false;
                currentLadder = null;
                rb.gravityScale = 2.5f;
            }
        }
    }

    // Start attack animation
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

        GameObject screenCanvas = GameObject.Find("ScreenCanvas");
        Transform child = screenCanvas.transform.Find("YouDied").set
    }
}