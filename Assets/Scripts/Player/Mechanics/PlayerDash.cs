using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MovementInput movementInput;

    [Header("Dash")]
    [SerializeField,Range(1f,10f)] private float dashDistance;
    [SerializeField,Range(0.1f, 1f)] private float dashTime;
    [SerializeField,Range(0f, 10f)] private float dashCooldown;
    [SerializeField, Range(0f, 50f)] private float dashResistance;
    [Space]
    [SerializeField] private bool disableGravity;
    [SerializeField] private bool oneDashPerGrounded;

    private float dashCooldownTimer;
    private float dashPerformTimer;
    private bool hasTouchedGround;
    public bool IsDashing { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private bool DashPressed => movementInput.GetDashDown();
    private bool shouldDash;

    private float previousGravityScale;
    private float currentDashDirection;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DashUpdateLogic();
        CheckTouchedGround();
    }

    private void FixedUpdate()
    {
        DashFixedUpdateLogic();
        DashResistance();
    }

    private void DashUpdateLogic()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        if (dashPerformTimer > 0) dashPerformTimer -= Time.deltaTime;
        else if (IsDashing) StopDash();

        if (oneDashPerGrounded && !hasTouchedGround) return;

        if (DashPressed && dashCooldownTimer <= 0) shouldDash = true;
    }
    private void DashFixedUpdateLogic()
    {
        if (shouldDash)
        {
            Dash();
            shouldDash = false;
            dashCooldownTimer = dashCooldown;
            dashPerformTimer = dashTime;
            hasTouchedGround = false;
        }
    }

    public void Dash()
    {
        if (disableGravity)
        {
            previousGravityScale = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;
        }

        currentDashDirection = DefineDashDirection();

        float dashForce = dashDistance / dashTime;

        _rigidbody2D.velocity = new Vector2(currentDashDirection * dashForce, 0f);
        IsDashing = true;
    }

    private void StopDash()
    {
        if (disableGravity)
        {
            _rigidbody2D.gravityScale = previousGravityScale;
        }

        _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
        IsDashing = false;
    }

    private void DashResistance()
    {
        if (IsDashing)
        {
            _rigidbody2D.AddForce(new Vector2(-currentDashDirection * dashResistance,0f), ForceMode2D.Force);
        }
    }

    private void CheckTouchedGround()
    {
        if (IsDashing) return;
        if (!checkGround.IsGrounded) return;

        hasTouchedGround = true;
    }

    private float DefineDashDirection()
    {
        if (playerMovement.LastNonZeroInput == 0f) return 1f;
        return playerMovement.LastNonZeroInput;
    }
}