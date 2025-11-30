using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Data")]
    public PlayerMovementData movementData;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.25f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Jump Buffer")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    private bool hasJumped = false;
    private bool wasFalling = false;

    [Header("Dash Settings")]
    public float dashPower = 18f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 0.4f;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    private bool facingRight = true;
    private Rigidbody2D rb;
    private float targetVelocityX = 0f;
    private float baseGravityScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        if (movementData != null)
        {
            baseGravityScale = movementData.gravityScale;
            rb.gravityScale = baseGravityScale;
        }
        else
        {
            baseGravityScale = rb.gravityScale;
        }
    }

    void Update()
    {
        GroundCheck();

        if (rb.linearVelocity.y < -1f)
        {
            wasFalling = true;
        }

        bool reallyGrounded = isGrounded && wasFalling && Mathf.Abs(rb.linearVelocity.y) < 0.5f;

        if (reallyGrounded && hasJumped)
        {
            hasJumped = false;
            wasFalling = false;
            coyoteTimeCounter = coyoteTime;
        }
        else if (!hasJumped && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        HandleJump();
        HandleDashInput();

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyBetterGravity();
    }

    void GroundCheck()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    void HandleMovement()
    {
        if (isDashing || movementData == null) return;

        float input = Input.GetAxisRaw("Horizontal");
        float speed = movementData.moveSpeed;

        targetVelocityX = speed * input;

        rb.linearVelocity = new Vector2(
            Mathf.Lerp(rb.linearVelocity.x, targetVelocityX, 25f * Time.fixedDeltaTime),
            rb.linearVelocity.y
        );

        if (input > 0 && !facingRight) Flip();
        else if (input < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    void HandleJump()
    {
        if (movementData == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (coyoteTimeCounter > 0f && !hasJumped)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector2.up * movementData.jumpForce, ForceMode2D.Impulse);
                coyoteTimeCounter = 0f;
                hasJumped = true;
            }
        }
    }

    void ApplyBetterGravity()
    {
        if (isDashing || movementData == null) return;

        if (rb.linearVelocity.y < 0)
            rb.gravityScale = baseGravityScale * 2.5f;
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.gravityScale = baseGravityScale * 2f;
        else
            rb.gravityScale = baseGravityScale;
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldownTimer <= 0 && !isDashing)
            StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        float dir = Input.GetAxisRaw("Horizontal");
        if (dir == 0) dir = facingRight ? 1 : -1;

        rb.linearVelocity = new Vector2(dir * dashPower, 0);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);

        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }
}