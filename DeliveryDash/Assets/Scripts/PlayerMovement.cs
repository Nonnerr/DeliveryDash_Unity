using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    //Movement
    public float walkSpeed = 8f;
    public float runSpeed = 12f;
    public float accelerationFactor = 25f;
    private bool facingRight = true;

    //Jumping
    public float jumpForce = 16f;
    public float coyoteTimeDuration = 0.15f;
    public Transform groundCheck;
    public float groundRadius = 0.25f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private int maxJumps = 2;
    private int jumpCount = 0;

    //Gravity
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    private float baseGravityScale;

    //Dash
    public float dashPower = 18f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 0.4f;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    private Rigidbody2D rb;
    private float targetVelocityX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        baseGravityScale = rb.gravityScale;
    }

    void Update()
    {
        GroundCheck();
        HandleJumpInput();
        HandleDashInput();

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        HandleMovementFixed();
        ApplyBetterGravity();
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded)
        {
            jumpCount = 0;
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void HandleMovementFixed()
    {
        if (isDashing) return;

        float input = Input.GetAxisRaw("Horizontal");
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        targetVelocityX = currentSpeed * input;

        rb.linearVelocity = new Vector2(
            Mathf.Lerp(rb.linearVelocity.x, targetVelocityX, accelerationFactor * Time.fixedDeltaTime),
            rb.linearVelocity.y
        );

        if (input > 0 && !facingRight)
        {
            Flip();
        }
        else if (input < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || coyoteTimeCounter > 0)
            {
                
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpCount = 1;
                coyoteTimeCounter = 0; 
            }
            else if (jumpCount > 0 && jumpCount < maxJumps)
            {
             
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpCount++;
            }
        }
    }

    void ApplyBetterGravity()
    {
        if (isDashing) return;

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravityScale * fallMultiplier;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.gravityScale = baseGravityScale * lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = baseGravityScale;
        }
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dir = Input.GetAxisRaw("Horizontal");

  
        if (dir == 0)
        {
            dir = transform.localScale.x;
        }

        rb.linearVelocity = new Vector2(dir * dashPower, 0);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);

        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}