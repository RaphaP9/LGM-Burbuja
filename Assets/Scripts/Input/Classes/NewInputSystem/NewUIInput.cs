using System;
using UnityEngine;

public class NewUIInput : UIInput
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
        playerInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.UI.Disable();
    }

    public override bool CanProcessInput()
    {
        return true;
    }

    public override bool GetPauseDown()
    {
        if (!CanProcessInput()) return false;
        if (InputOnCooldown()) return false;

        bool pauseInput = playerInputActions.UI.Pause.WasPerformedThisFrame();
        return pauseInput;
    }
}
