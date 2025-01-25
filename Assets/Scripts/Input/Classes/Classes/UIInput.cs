using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIInput : MonoBehaviour, IUIInput
{
    public static UIInput Instance { get; private set; }

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
    public abstract bool GetPauseDown();
}
