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

    [Header("Setting")]
    [SerializeField, Range(0.01f, 100f)] private float smoothAtractionFactor;

    public bool IsOnBubble ;//{ get; private set; }

    public Bubble currentBubble;

    private float previousGravityScale;

    private Rigidbody2D _rigidbody2D;
    private Vector2 positionVector2;

    private float bubbleTimer;

    public static event EventHandler OnBubbleAttach;
    public static event EventHandler OnBubbleUnattach;

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

    private void FixedUpdate()
    {
        HandleBubbleCenterAtraction();
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
    private void Bubble_OnBubbleEnter(object sender, Bubble.OnBubbleEventArgs e)
    {
        OnBubbleAttach?.Invoke(this, EventArgs.Empty);

        previousGravityScale = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;
        SetCurrentBubble(e.bubble);
        IsOnBubble = true;

        Debug.Log("Enter");
    }
    private void ResetBubbleTimer() => bubbleTimer = 0f;

    private void Bubble_OnBubbleReleased(object sender, Bubble.OnBubbleEventArgs e)
    {
        if (currentBubble != e.bubble) return;

        _rigidbody2D.gravityScale = previousGravityScale;
        ClearCurrentBubble();
        IsOnBubble = false;

        ResetBubbleTimer();

        OnBubbleUnattach?.Invoke(this, EventArgs.Empty);

        Debug.Log("Exit");

        playerGravityController.ResetYVelocity();
    }

    private void PlayerJump_OnPlayerJump(object sender, PlayerJump.OnPlayerJumpEventArgs e)
    {
        if (currentBubble == null) return;

        _rigidbody2D.gravityScale = previousGravityScale;
        ClearCurrentBubble();
        IsOnBubble = false;

        ResetBubbleTimer();

        OnBubbleUnattach?.Invoke(this, EventArgs.Empty);

        Debug.Log("Exit");

        playerGravityController.ResetYVelocity();
    }

    private void PlayerDash_OnPlayerDashPre(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        if (currentBubble == null) return;

        _rigidbody2D.gravityScale = previousGravityScale;
        ClearCurrentBubble();
        IsOnBubble = false;

        ResetBubbleTimer();

        OnBubbleUnattach?.Invoke(this, EventArgs.Empty);

        Debug.Log("Exit");

        playerGravityController.ResetYVelocity();
    }

}
