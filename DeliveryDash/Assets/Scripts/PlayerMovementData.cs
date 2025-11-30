using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "DeliveryDash/Player Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.15f;

    [Header("Physics")]
    public float gravityScale = 1.5f;
    public float fallGravityMultiplier = 2f;
    public float lowJumpGravityMultiplier = 2.5f;

    [Header("Dash")]
    public float dashPower = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
}
