using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJump : MonoBehaviour
{
    [Header("Enabler")]
    [SerializeField] private bool jumpEnabled;

    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [Space]
    [SerializeField] private PlayerGravityController playerGravityController;
    [Space]
    [SerializeField] private CheckGround checkGround;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpHeightError = 0.05f;
    [SerializeField, Range(0f, 0.5f)] private float impulseTime = 0.2f;
    [SerializeField, Range(0f, 1.5f)] private float jumpCooldown = 1f;
    [SerializeField, Range(0f, 1.5f)] private float jumpCooldownGround = 1f;
    [SerializeField, Range(0,3)] private int jumpLimit;

    private Rigidbody2D _rigidbody2D;
    private enum State { NotJumping, Impulsing, Jump }
    private State state;
    public bool NotJumping => state == State.NotJumping;

    private bool JumpInput => movementInput.GetJumpDown();

    private float jumpCooldownTime = 0f;
    private float timer = 0f;
    private bool shouldJump;

    public bool JumpEnabled { get { return jumpEnabled; } }

    private int jumpsPerformed;

    public static event EventHandler OnPlayerImpulsing;
    public static event EventHandler<OnPlayerJumpEventArgs> OnPlayerJump;

    public class OnPlayerJumpEventArgs : EventArgs
    {
        public int jumpsPerformed;
    }

    private void OnEnable()
    {
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
    }

    private void OnDisable()
    {
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        if (!jumpEnabled) return;

        CheckShouldJump();
        HandleJumpStates();
    }

    private void InitializeVariables()
    {
        jumpCooldownTime = 0f;
    }

    private void CheckShouldJump()
    {
        if (shouldJump) return;
        if (!HasJumpsLeft()) return;
        if (JumpOnCooldown()) return;
        if (!JumpInput) return;

        shouldJump = true;
    }

    private void SetJumpState(State state) { this.state = state; }
    public void HandleJumpStates()
    {
        switch (state)
        {
            case State.NotJumping:
                NotJumpingLogic();
                break;
            case State.Impulsing:
                ImpulsingLogic();
                break;
            case State.Jump:
                JumpLogic();
                break;
        }
    }

    private void NotJumpingLogic()
    {
        if (shouldJump)
        {
            _rigidbody2D.gravityScale = 0f;
            ResetTimer();
            SetJumpState(State.Impulsing);
            OnPlayerImpulsing?.Invoke(this, EventArgs.Empty);
        }

        HandleJumpCooldown();
    }

    private void ImpulsingLogic()
    {
        timer += Time.deltaTime;

        //_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

        if (timer >= impulseTime)
        {
            ResetTimer();
            SetJumpState(State.Jump);
        }
    }

    private void JumpLogic()
    {
        OnPlayerJump?.Invoke(this, new OnPlayerJumpEventArgs { jumpsPerformed = jumpsPerformed});

        _rigidbody2D.gravityScale = 1f;

        AddJumpsPerformed(1);
        Jump();
        SetJumpCooldown(jumpCooldown);

        ResetTimer();
        SetJumpState(State.NotJumping);
    }

    private void Jump()
    {
        shouldJump = false;
        playerGravityController.ResetYVelocity();
        float jumpForce = CalculateJumpForce(jumpHeight, Physics2D.gravity.y * playerGravityController.GravityMultiplier * playerGravityController.LowJumpMultiplier);
        _rigidbody2D.AddForce(new Vector2(0f, jumpForce + jumpHeightError), ForceMode2D.Impulse);
    }

    private float CalculateJumpForce(float jumpHeight, float gravity)
    {
        float jumpForce = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
        return jumpForce;
    }

    private void HandleJumpCooldown()
    {
        //if (!checkGround.IsGrounded) return;

        jumpCooldownTime = jumpCooldownTime > 0f ? jumpCooldownTime -= Time.deltaTime : 0f;
    }

    private bool JumpOnCooldown() => jumpCooldownTime > 0f;
    private void SetJumpCooldown(float cooldown) => jumpCooldownTime = cooldown;
    private void ResetTimer() => timer = 0f;

    private float SetJumpsPerformed(int jumpsPerformed) => this.jumpsPerformed = jumpsPerformed;

    private void AddJumpsPerformed(int quantity) => jumpsPerformed += quantity;

    private bool HasJumpsLeft() => jumpsPerformed < jumpLimit;

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        SetJumpsPerformed(0);
        SetJumpCooldown(jumpCooldownGround);
    }
}
