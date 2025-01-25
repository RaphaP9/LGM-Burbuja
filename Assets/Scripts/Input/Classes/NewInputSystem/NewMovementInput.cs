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

    public override float GetMovementInputNormalized()
    {
        if (!CanProcessInput()) return 0f;

        float input = playerInputActions.Movement.Move.ReadValue<float>();

        return input;
    }
}
