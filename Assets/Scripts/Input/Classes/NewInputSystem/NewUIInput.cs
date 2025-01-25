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


    public override bool CanProcessInput()
    {
        return true;
    }

    public override bool GetPauseDown()
    {
        if (!CanProcessInput()) return false;

        bool pauseInput = playerInputActions.UI.Pause.WasPerformedThisFrame();
        return pauseInput;
    }
}
