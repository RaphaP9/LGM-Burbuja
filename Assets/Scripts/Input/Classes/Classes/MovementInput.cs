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
    public abstract Vector2 GetMovementVectorNormalized();
}
