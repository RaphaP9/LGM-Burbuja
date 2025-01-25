using System;
using UnityEngine;

public class NewMovementInput : MovementInput
{
    private PlayerInputActions playerInputActions;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movement.Enable();
    }


    public override bool CanProcessInput()
    {
        return true;
    }

    public override Vector2 GetMovementVectorNormalized()
    {
        if (!CanProcessInput()) return Vector2.zero;

        Vector2 inputVector = playerInputActions.Movement.Move.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }
}
