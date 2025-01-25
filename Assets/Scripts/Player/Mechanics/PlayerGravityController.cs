using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerMovement playerHorizontalMovement;
    [SerializeField] private MovementInput movementInput;

    [Header("Physic Materials")]
    [SerializeField] private PhysicsMaterial2D frictionMaterial;
    [SerializeField] private PhysicsMaterial2D frictionlessMaterial;

    [Header("Gravity Settings")]
    [SerializeField, Range(0.5f, 2f)] private float gravityMultiplier = 1f;
    [SerializeField, Range(0.5f, 3f)] private float fallMultiplier;
    [SerializeField, Range(0.5f, 3f)] private float lowJumpMultiplier;
    [SerializeField] private bool useBetterJump;

    [Header("Stick To Slope Settings")]
    [SerializeField] private bool enableStickToSlopeForce;
    [SerializeField] private float stickToSlopeSpeedThreshold;
    [SerializeField, Range(0f, 100f)] private float stickToSlopeForce = 5f;

    public float HorizontalSpeed => Math.Abs(playerHorizontalMovement.FinalMoveValue);

    public float GravityMultiplier => gravityMultiplier;
    public float FallMultiplier => fallMultiplier;
    public float LowJumpMultiplier => lowJumpMultiplier;

    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D capsulleCollider2D;

    private bool previousWasOnSlope;
    private bool currentIsOnSlope;

    private Vector2 currentSlopeNormal;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        capsulleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        currentIsOnSlope = checkGround.OnSlope;
        previousWasOnSlope = currentIsOnSlope;

        currentSlopeNormal = checkGround.SlopeNormal;
    }

    private void FixedUpdate()
    {
        BetterFall();
        HandleSlopes();

    }

    private void BetterFall()
    {
        if (checkGround.IsGrounded) return;

        if (_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.velocity += Vector2.up * Physics.gravity.y * GravityMultiplier * (FallMultiplier - 1) * Time.fixedDeltaTime;
        }

        else if (_rigidbody2D.velocity.y > 0 && (!(movementInput.GetJump() && useBetterJump)))
        {
            _rigidbody2D.velocity += Vector2.up * Physics.gravity.y * GravityMultiplier * (LowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void HandleSlopes()
    {
        currentIsOnSlope = checkGround.OnSlope;

        if (currentIsOnSlope && !previousWasOnSlope)
        {
            EnterSlope();
        }

        if (!currentIsOnSlope && previousWasOnSlope)
        {
            LeaveSlope();
        }

        if (currentIsOnSlope)
        {
            StayOnSlope();
        }

        previousWasOnSlope = currentIsOnSlope;
    }

    private void EnterSlope()
    {
        capsulleCollider2D.sharedMaterial = frictionMaterial;
    }

    private void StayOnSlope()
    {
        if (!enableStickToSlopeForce) return;
        if (HorizontalSpeed < stickToSlopeSpeedThreshold) return;

        _rigidbody2D.AddForce(stickToSlopeForce * -checkGround.SlopeNormal, ForceMode2D.Force);
    }

    private void LeaveSlope()
    {
        capsulleCollider2D.sharedMaterial = frictionlessMaterial;
    }

}
