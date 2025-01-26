using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJump : MonoBehaviour
{
    public static PlayerJump Instance {  get; private set; }

    [Header("Enabler")]
    [SerializeField] private bool jumpEnabled;

    [Header("Components")]
    [SerializeField] private PlayerGravityController playerGravityController;
    [SerializeField] private PlayerBubbleHandler playerBubbleHandler;
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private MovementInput movementInput;

    [Header("Jump Settings")]
    [SerializeField,Range(1f,10f)] private float jumpHeight = 5f;
    [SerializeField, Range(1f, 10f)] private float jumpHeightBubble = 5f;
    [SerializeField,Range(0f,0.1f)] private float jumpHeightError = 0.05f;
    [SerializeField, Range(0f, 0.5f)] private float impulseTime = 0.2f;
    [SerializeField, Range(0f, 1.5f)] private float jumpCooldown = 1f;
    [SerializeField, Range(0f, 1.5f)] private float jumpCooldownGround = 1f;
    [SerializeField, Range(0f, 1.5f)] private float jumpCooldownBubble = 1f;
    [SerializeField, Range(0,3)] private int jumpLimit;
    [Space]
    [SerializeField] private bool resetJumpsOnBubble;

    private Rigidbody2D _rigidbody2D;
    private enum State { NotJumping, Impulsing, Jump }
    private State state;
    public bool NotJumping => state == State.NotJumping;

    private bool JumpInput => movementInput.GetJumpDown();

    private float jumpCooldownTime = 0f;
    private float timer = 0f;
    private bool shouldJump;

    public bool JumpEnabled { get { return jumpEnabled; } }

    public int jumpsPerformed;

    public static event EventHandler OnPlayerImpulsing;
    public static event EventHandler<OnPlayerJumpEventArgs> OnPlayerJump;

    public bool nextJumpFromBubble = false;

    public class OnPlayerJumpEventArgs : EventArgs
    {
        public int jumpsPerformed;
        public bool fromBubble;
    }

    private void OnEnable()
    {
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
        PlayerBubbleHandler.OnBubbleAttach += PlayerBubbleHandler_OnBubbleAttach;
        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
    }
    private void OnDisable()
    {
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;
        PlayerBubbleHandler.OnBubbleAttach -= PlayerBubbleHandler_OnBubbleAttach;
        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;

    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetSingleton();
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

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerJump instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
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
        //if (playerBubbleHandler.IsOnBubble) return;

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
        OnPlayerJump?.Invoke(this, new OnPlayerJumpEventArgs { jumpsPerformed = jumpsPerformed, fromBubble = nextJumpFromBubble});

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

        float derisedJumpHeight = nextJumpFromBubble ? jumpHeightBubble : jumpHeight;

        float jumpForce = CalculateJumpForce(derisedJumpHeight, Physics2D.gravity.y * playerGravityController.GravityMultiplier * playerGravityController.LowJumpMultiplier);
        _rigidbody2D.AddForce(new Vector2(0f, jumpForce + jumpHeightError), ForceMode2D.Impulse);

        nextJumpFromBubble = false;
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


        if (playerBubbleHandler.IsOnBubble) return;
        nextJumpFromBubble = false;
    }

    private void PlayerBubbleHandler_OnBubbleAttach(object sender, EventArgs e)
    {
        if (!resetJumpsOnBubble) return;

        SetJumpsPerformed(0);
        SetJumpCooldown(jumpCooldownBubble);

        nextJumpFromBubble = true;
    }

    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        if (e.fromBubble) nextJumpFromBubble = false;
    }

    public void UnlockDoubleJump() => jumpLimit = 2;
}
