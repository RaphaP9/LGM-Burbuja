using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementInput : MonoBehaviour, IMovementInput
{
    public static MovementInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one MovementInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessInput();
    public abstract float GetMovementInputNormalized();

    public abstract bool GetJumpDown();
    public abstract bool GetJump();
    public abstract bool GetDashDown();
}
