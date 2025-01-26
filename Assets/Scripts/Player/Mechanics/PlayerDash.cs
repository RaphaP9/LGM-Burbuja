using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Enabler")]
    [SerializeField] private bool dashEnabled;

    [Header("Components")]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerBubbleHandler playerBubbleHandler;
    [SerializeField] private MovementInput movementInput;

    [Header("Dash")]
    [SerializeField,Range(1f,10f)] private float dashDistance;
    [SerializeField,Range(1f,10f)] private float dashDistanceFromBubble;
    [Space]
    [SerializeField,Range(0.1f, 1f)] private float dashTime;
    [SerializeField,Range(0.1f, 1f)] private float dashTimeFromBubble;
    [SerializeField,Range(0f, 10f)] private float dashCooldown;
    [SerializeField, Range(0f, 50f)] private float dashResistance;
    [Space]
    [SerializeField] private bool disableGravity;
    [SerializeField,Range(0,3)] private int dashLimit;
    [Space]
    [SerializeField] private bool resetDashesOnBubble;

    private float dashCooldownTimer;
    private float dashPerformTimer;

    public bool IsDashing { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private bool DashPressed => movementInput.GetDashDown();
    private bool shouldDash;

    private float previousGravityScale;
    private float currentDashDirection;

    public int dashesPerformed;

    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDash;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashPre;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashStopped;

    private bool nextDashFromBubble = false;
    public class OnPlayerDashEventArgs : EventArgs
    {
        public float dashDirection;
        public int dashesPerformed;
        public bool fromBubble;
    }

    private void OnEnable()
    {
        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerBubbleHandler.OnBubbleAttach += PlayerBubbleHandler_OnBubbleAttach;
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
    }

    private void OnDisable()
    {
        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerBubbleHandler.OnBubbleAttach -= PlayerBubbleHandler_OnBubbleAttach;
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;
    }


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetDashesPerformed(0);
    }

    private void Update()
    {
        if (!dashEnabled) return;

        HandleDashCooldown();
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
        
        if (dashPerformTimer > 0) dashPerformTimer -= Time.deltaTime;
        else if (IsDashing) StopDash();

        if (!HasDashesLeft()) return;
        //if (playerBubbleHandler.IsOnBubble) return;

        if (DashPressed && dashCooldownTimer <= 0) shouldDash = true;
    }

    private void HandleDashCooldown()
    {
        if (dashCooldownTimer > 0 && !IsDashing) dashCooldownTimer -= Time.deltaTime;
    }

    private void DashFixedUpdateLogic()
    {
        if (shouldDash)
        {
            AddDashesPerformed(1);
            Dash();
            shouldDash = false;
            SetDashCooldownTimer(dashCooldown);
            dashPerformTimer = dashTime;
        }
    }

    public void Dash()
    {
        OnPlayerDashPre?.Invoke(this, new OnPlayerDashEventArgs { dashesPerformed = dashesPerformed, dashDirection = currentDashDirection, fromBubble = nextDashFromBubble });

        if (disableGravity)
        {
            previousGravityScale = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;
        }

        currentDashDirection = DefineDashDirection();

        float dashForce = dashDistance / dashTime;
        float dashForceFromBubble = dashDistanceFromBubble / dashTimeFromBubble;

        float desiredDashForce = nextDashFromBubble ? dashForceFromBubble : dashForce;

        _rigidbody2D.velocity = new Vector2(currentDashDirection * desiredDashForce, 0f);
        IsDashing = true;

        OnPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashesPerformed = dashesPerformed , dashDirection = currentDashDirection, fromBubble = nextDashFromBubble });

        nextDashFromBubble = false;
    }

    private void StopDash()
    {
        if (!IsDashing) return;
        
        if (disableGravity)
        {
            _rigidbody2D.gravityScale = previousGravityScale;
        }

        _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
        IsDashing = false;

        OnPlayerDashStopped?.Invoke(this, new OnPlayerDashEventArgs { dashesPerformed = dashesPerformed });
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

        SetDashesPerformed(0);
    }

    private float DefineDashDirection()
    {
        if (playerMovement.LastNonZeroInput != 0f) return playerMovement.LastNonZeroInput;
        if (movementInput.GetMovementInputNormalized() != 0f) return movementInput.GetMovementInputNormalized();
        return 1f;
    }

    private float SetDashesPerformed(int dashesPerformed) => this.dashesPerformed = dashesPerformed;

    private void AddDashesPerformed(int quantity) => dashesPerformed += quantity;

    private bool HasDashesLeft() => dashesPerformed < dashLimit;

    private void SetDashCooldownTimer(float cooldown) => dashCooldownTimer = cooldown;

    private void PlayerJump_OnPlayerJump(object sender, PlayerJump.OnPlayerJumpEventArgs e)
    {
        StopDash();

        if (e.fromBubble) nextDashFromBubble = false;
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        nextDashFromBubble = false;
    }

    private void PlayerBubbleHandler_OnBubbleAttach(object sender, EventArgs e)
    {
        StopDash();

        if (!resetDashesOnBubble) return;
        SetDashesPerformed(0);
        SetDashCooldownTimer(dashCooldown);

        nextDashFromBubble = true;

    }
}