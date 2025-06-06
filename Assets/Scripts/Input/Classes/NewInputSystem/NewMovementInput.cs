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

    private void OnDisable()
    {
        playerInputActions.Movement.Disable();
    }


    public override bool CanProcessInput()
    {
        if (GameManager.Instance.GameState != GameManager.State.OnGameplay) return false;
        return true;
    }

    public override float GetMovementInputNormalized()
    {
        if (!CanProcessInput()) return 0f;

        float input = playerInputActions.Movement.Move.ReadValue<float>();

        return input;
    }

    public override bool GetJumpDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Movement.Jump.WasPerformedThisFrame();

        return input;
    }

    public override bool GetJump()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Movement.Jump.IsPressed();

        return input;
    }

    public override bool GetDashDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Movement.Dash.WasPerformedThisFrame();

        return input;
    }

    public override bool GetReleaseDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Movement.Release.WasPerformedThisFrame();

        return input;
    }
}
