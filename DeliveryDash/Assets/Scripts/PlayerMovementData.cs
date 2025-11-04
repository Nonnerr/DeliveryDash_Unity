using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "DeliveryDash/Player Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    [Header("Physics")]
    public float gravityScale = 1.5f;
}
