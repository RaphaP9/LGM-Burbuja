using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIInput : MonoBehaviour, IUIInput
{
    public static UIInput Instance { get; private set; }

    [Header("Settings")]
    [SerializeField,Range(0f,1f)] protected float inputCooldown;

    protected float inputCooldownTimer;

    protected virtual void Awake()
    {
        SetSingleton();
    }
    private void Start()
    {
        ResetInputCooldownTimer();
    }

    private void Update()
    {
        HandleInputCooldown();
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

    public void UseInput() => MaxInputCooldownTimer();

    private void HandleInputCooldown()
    {
        if (inputCooldownTimer > 0f) inputCooldownTimer -= Time.unscaledDeltaTime;
    }
    private void MaxInputCooldownTimer() => inputCooldownTimer = inputCooldown;
    private void ResetInputCooldownTimer() => inputCooldownTimer = 0f;
    protected bool InputOnCooldown() => inputCooldownTimer > 0f;
}
