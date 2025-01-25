using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLand : MonoBehaviour
{
    public static PlayerLand Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private PlayerFall playerFall;
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private PlayerGravityController playerGravityController;

    [Header("Land Settings")]
    [SerializeField, Range(0f, 1.5f)] private float landDetectionHeightThreshold;
    [SerializeField, Range(0f, 1f)] private float landRecoveryTime;

    public bool IsRecoveringFromLanding { get; private set; }

    private enum State {NotLanding,Landing}
    private State state;

    private Rigidbody2D _rigidbody2D;
    private bool previouslyGrounded;
    private float timer = 0f;

    public static event EventHandler<OnPlayerLandEventArgs> OnPlayerLand;

    private float lastNonZeroVelocityY = 0f;

    public class OnPlayerLandEventArgs : EventArgs
    {
        public float landHeight;
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
        HandleLandStates();

        UpdateLastNonZeroVelocityY();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerLand instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        previouslyGrounded = checkGround.IsGrounded;
    }

    private void HandleLandStates()
    {
        switch (state)
        {
            case State.NotLanding:
                NotLandingLogic();
                break;
            case State.Landing:
                LandingLogic();
                break;
        }

        previouslyGrounded = checkGround.IsGrounded;
    }

    private void SetLandState(State state) { this.state = state; }

    private void NotLandingLogic()
    {
        IsRecoveringFromLanding = false;

        if (!previouslyGrounded && checkGround.IsGrounded)
        {
            float landHeight = CalculateLandHeight(lastNonZeroVelocityY, _rigidbody2D.gravityScale * playerGravityController.GravityMultiplier * playerGravityController. FallMultiplier);

            if (landHeight <= 0f) return;

            if (HasSurpassedThreshold()) OnPlayerLand?.Invoke(this, new OnPlayerLandEventArgs { landHeight = landHeight});

            ResetTimer();

            SetLandState(State.Landing);
        }
    }

    private void LandingLogic()
    {
        IsRecoveringFromLanding = true;

        timer += Time.deltaTime;

        if (timer >= landRecoveryTime)
        {
            ResetTimer();
            SetLandState(State.NotLanding);
        }
    }

    private float CalculateLandVelocity(float height, float gravity)
    {
        float landVelocity = - Mathf.Sqrt(2 * height * Mathf.Abs(gravity));
        return landVelocity;
    }

    private float CalculateLandHeight(float velocity, float gravity)
    {
        float landHeight = Mathf.Pow(Mathf.Abs(velocity), 2) / (2 * Mathf.Abs(gravity));
        return landHeight;
    }

    private bool HasSurpassedThreshold()
    {
        return lastNonZeroVelocityY <= CalculateLandVelocity(landDetectionHeightThreshold, Physics.gravity.y * playerGravityController.GravityMultiplier * playerGravityController.FallMultiplier);
    }

    private void ResetTimer() => timer = 0f;

    private void UpdateLastNonZeroVelocityY()
    {
        lastNonZeroVelocityY = _rigidbody2D.velocity.y;
    }
}
