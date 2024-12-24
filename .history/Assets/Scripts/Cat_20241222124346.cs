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
    public bool isClimbing = false; // This will be controlled by the Climbable script

    // Attack state variable
    public CatHitBox hitBox;
    private bool isAttacking = false;

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

        currentSkin = GameManager.Instance.currentCatSkin;
        animator.runtimeAnimatorController = currentSkin;

        isAlive = true;
    }

    void Update()
    {
        if (isAttacking) return;

        if (!isClimbing)
        {
            // Normal movement logic
            moveInput = Input.GetAxisRaw("Horizontal");

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
                playerFacingDirection = Mathf.Sign(moveInput);
            }

            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetFloat("MoveX", moveInput);
        }
        else
        {
            // Reset animations while climbing
            animator.SetFloat("Speed", 0);
            animator.SetFloat("MoveX", 0);
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.R) && !isAttacking)
        {
            StartAttack();
        }
    }

    void FixedUpdate()
    {
        if (isAttacking || isClimbing) return;

        // Normal movement
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    // Rest of the methods (OnCollisionStay2D, OnCollisionExit2D, StartAttack, etc.) remain the same
    // Remove all ladder/climbing related methods as they're now handled by Climbable
}