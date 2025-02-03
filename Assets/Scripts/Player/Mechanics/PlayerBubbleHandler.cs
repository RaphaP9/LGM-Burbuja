using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBubbleHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerGravityController playerGravityController;
    [SerializeField] private Transform attachPoint;
    [SerializeField] private MovementInput movementInput;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 100f)] private float smoothAtractionFactor;
    [SerializeField,Range(0f,2f)] private float previousBubbleAttachCooldown;

    public bool ReleaseInput => movementInput.GetReleaseDown();

    public bool IsOnBubble ;//{ get; private set; }

    public Bubble currentBubble;
    public Bubble previousBubble;

    private float previousGravityScale;

    private Rigidbody2D _rigidbody2D;
    private Vector2 positionVector2;

    private float bubbleTimer;
    public float previousBubbleAttachCooldownTimer;

    public static event EventHandler OnBubbleAttachPre;
    public static event EventHandler<OnBubbleEventArgs> OnBubbleAttach;
    public static event EventHandler<OnBubbleEventArgs> OnBubbleUnattach;

    private const float ORIGINAL_GRAVITY_SCALE = 1f;

    public class OnBubbleEventArgs : EventArgs
    {
        public Bubble bubble;
    }

    private void OnEnable()
    {
        Bubble.OnBubbleEnter += Bubble_OnBubbleEnter;
        Bubble.OnBubbleReleased += Bubble_OnBubbleReleased;

        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerDash.OnPlayerDashPre += PlayerDash_OnPlayerDashPre;
    }

    private void OnDisable()
    {
        Bubble.OnBubbleEnter -= Bubble_OnBubbleEnter;
        Bubble.OnBubbleReleased -= Bubble_OnBubbleReleased;

        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerDash.OnPlayerDashPre -= PlayerDash_OnPlayerDashPre;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetBubbleTimer();
    }

    private void Update()
    {
        HandlePreviousBubbleAttachCooldown();

        HandleReleaseBubble();
    }

    private void FixedUpdate()
    {
        HandleBubbleCenterAtraction();
    }

    private void HandlePreviousBubbleAttachCooldown()
    {
        if (PreviousBubbleAttachOnCooldown())
        {
            previousBubbleAttachCooldownTimer -= Time.deltaTime;
            return;
        }
    }

    private void HandleReleaseBubble()
    {
        Bubble previousBubble = currentBubble;

        if (!ReleaseInput) return;

        if (currentBubble == null) return;

        _rigidbody2D.gravityScale = previousGravityScale;
        ClearCurrentBubble();
        IsOnBubble = false;

        ResetBubbleTimer();

        OnBubbleUnattach?.Invoke(this, new OnBubbleEventArgs { bubble = previousBubble});

        Debug.Log("Exit");

        playerGravityController.ResetYVelocity();

        SetPreviousBubbleAttachCooldown(previousBubbleAttachCooldown);
    }

    private void HandleBubbleCenterAtraction()
    {
        if (currentBubble == null) return;

        bubbleTimer += Time.fixedDeltaTime;

        Vector2 targetPosition = CalculateTargetPosition();

        Vector2 newPosition = Vector2.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothAtractionFactor);

        _rigidbody2D.MovePosition(newPosition);
    }

    private Vector2 CalculateTargetPosition()
    {
        Vector2 targetPosition = currentBubble.BubbleCentrer.position + (transform.position - attachPoint.position);
        return targetPosition;
    }

    private void SetCurrentBubble(Bubble bubble) => currentBubble = bubble; 
    private void ClearCurrentBubble() => currentBubble = null;
    private void SetPreviousBubble(Bubble bubble) => previousBubble = bubble;
    private void ClearPreviousBubble() => previousBubble = null;
    private void ResetBubbleTimer() => bubbleTimer = 0f;

    private void ResetPreviousBubbleAttachCooldown() => previousBubbleAttachCooldownTimer = 0f;
    private void SetPreviousBubbleAttachCooldown(float cooldown) => previousBubbleAttachCooldownTimer = cooldown;
    private bool PreviousBubbleAttachOnCooldown() => previousBubbleAttachCooldownTimer > 0f;

    private void UnatachFromBubble()
    {
        if (currentBubble == null) return;

        SetPreviousBubble(currentBubble);
        ClearCurrentBubble();

        ResetBubbleTimer();

        _rigidbody2D.gravityScale = previousGravityScale;
        IsOnBubble = false;

        OnBubbleUnattach?.Invoke(this, new OnBubbleEventArgs { bubble = previousBubble });

        Debug.Log("Exit");

        playerGravityController.ResetYVelocity();

        SetPreviousBubbleAttachCooldown(previousBubbleAttachCooldown);
    }

    private void ReleaseFromBubble(Bubble.OnBubbleEventArgs e)
    {
        if (currentBubble != e.bubble) return;

        SetPreviousBubble(currentBubble);
        ClearCurrentBubble();

        ResetBubbleTimer();

        _rigidbody2D.gravityScale = previousGravityScale;
        IsOnBubble = false;

        OnBubbleUnattach?.Invoke(this, new OnBubbleEventArgs { bubble = previousBubble });

        Debug.Log("Exit");

        playerGravityController.ResetYVelocity();

        SetPreviousBubbleAttachCooldown(previousBubbleAttachCooldown);
    }


    #region Subscriptions
    private void Bubble_OnBubbleEnter(object sender, Bubble.OnBubbleEventArgs e)
    {
        EnterBubble(e);
    }

    private void EnterBubble(Bubble.OnBubbleEventArgs e)
    {
        if (PreviousBubbleAttachOnCooldown() && e.bubble == previousBubble) return;

        SetCurrentBubble(e.bubble);

        OnBubbleAttachPre?.Invoke(this, EventArgs.Empty);

        previousGravityScale = ORIGINAL_GRAVITY_SCALE;
        _rigidbody2D.gravityScale = 0f;
        IsOnBubble = true;

        OnBubbleAttach?.Invoke(this, new OnBubbleEventArgs { bubble = currentBubble });

        Debug.Log("Enter");
    }

    private void Bubble_OnBubbleReleased(object sender, Bubble.OnBubbleEventArgs e)
    {
        ReleaseFromBubble(e);
    }

    private void PlayerJump_OnPlayerJump(object sender, PlayerJump.OnPlayerJumpEventArgs e)
    {
        UnatachFromBubble();
    }

    private void PlayerDash_OnPlayerDashPre(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        UnatachFromBubble();
    }
    #endregion
}
