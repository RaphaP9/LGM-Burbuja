using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [Header("Enabler")]
    [SerializeField] private bool movementEnabled;

    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [Space]
    [SerializeField] private CheckGround checkGround;
    [SerializeField] private CheckWall checkWall;

    [Header("Movement Settings")]
    [SerializeField,Range(1f,10f)] private float walkSpeed;
    [SerializeField,Range(1f,10f)] private float sprintSpeed;
    [Space]
    [SerializeField] private bool flattenSpeedOnSlopes;
    [SerializeField, Range(0f, 10f)] private float flattenSpeedThreshold;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothInputFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothDirectionFactor = 5f;

    private Rigidbody2D _rigidbody2D;
    public float DirectionInput => movementInput.GetMovementInputNormalized();

    private float desiredSpeed;
    private float smoothCurrentSpeed;

    public float smoothDirectionInput;
    public float LastNonZeroInput { get; private set; }
    public float FinalMoveDir { get; private set; }
    public float SmoothFinalMoveInput { get; private set; }
    public float FinalMoveValue { get; private set; }
    public bool MovementEnabled => movementEnabled;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement(); 
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerMovement instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        if (!movementEnabled) return;

        CalculateDesiredSpeed();

        SmoothSpeed();
        CalculateLastNonZeroDirectionInput();
        
        SmoothDirectionInput();

        
        FlattenMovementInput();
        SmoothFinalInput();

        CalculateFinalMovement();
        
    }



    private void CalculateDesiredSpeed()
    {
        float moveSpeed = RestrictedMovement() ? walkSpeed : sprintSpeed;
        desiredSpeed = CanMove() ? moveSpeed : 0f;
    }

    private bool RestrictedMovement()
    {
        return false;
    }

    
    private bool CanMove()
    {
        if (DirectionInput == 0f) return false;
        if (checkWall.HitWall) return false;

        return true;
    }
    private void SmoothSpeed()
    {
        smoothCurrentSpeed = Mathf.Lerp(smoothCurrentSpeed, desiredSpeed, Time.deltaTime * smoothVelocityFactor);
    }

    private void CalculateLastNonZeroDirectionInput() => LastNonZeroInput = DirectionInput != 0f ? DirectionInput : LastNonZeroInput;

    private void SmoothDirectionInput() => smoothDirectionInput = Mathf.Lerp(smoothDirectionInput, LastNonZeroInput, Time.deltaTime * smoothInputFactor);

    private void FlattenMovementInput()
    {
        float flattenDir = FlattenVectorOnSlopes(smoothDirectionInput);

        FinalMoveDir = flattenDir;
    }

    private void SmoothFinalInput() => SmoothFinalMoveInput = Mathf.Lerp(SmoothFinalMoveInput, FinalMoveDir, Time.deltaTime * smoothDirectionFactor);

    private float FlattenVectorOnSlopes(float directionToFlat)
    {
        if (!checkGround.OnSlope) return directionToFlat;
        if (!flattenSpeedOnSlopes) return directionToFlat;
        if (FinalMoveValue < flattenSpeedThreshold) return directionToFlat;

        float dotProduct = Vector2.Dot(new Vector2(directionToFlat,0f), checkGround.SlopeNormal);  // Dot product of A and B
        float squaredLengthB = Vector2.Dot(checkGround.SlopeNormal, checkGround.SlopeNormal);  // Dot product of B with itself (length squared)

        Vector2 projected = (dotProduct / squaredLengthB) * checkGround.SlopeNormal;

        directionToFlat = projected.x;
        return directionToFlat;
    }

    private void CalculateFinalMovement()
    {
        float finalInput = SmoothFinalMoveInput * smoothCurrentSpeed;

        float roundedFinalInput;
        roundedFinalInput = Math.Abs(finalInput) < 0.01f ? 0f : finalInput;

        FinalMoveValue = roundedFinalInput;
    }

    private void ApplyMovement()
    {
        _rigidbody2D.velocity = new Vector2(FinalMoveValue, _rigidbody2D.velocity.y);
    }

}
