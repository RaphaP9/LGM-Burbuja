using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    public enum State { OnGameplay, OnUI, OnCutscene}

    public State GameState => state;

    private void OnEnable()
    {
        UIManager.OnUIActive += UIManager_OnUIActive;
        UIManager.OnUIInactive += UIManager_OnUIInactive;
    }

    private void OnDisable()
    {
        UIManager.OnUIActive -= UIManager_OnUIActive;
        UIManager.OnUIInactive -= UIManager_OnUIInactive;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetGameState(State.OnGameplay);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetGameState(State state)
    {
        previousState = this.state;
        this.state = state;
    }

    private void SetPreviousState(State state)
    {
        previousState = state;
    }

    #region UIManager Subcriptions
    private void UIManager_OnUIActive(object sender, System.EventArgs e)
    {
        SetGameState(State.OnUI);
    }

    private void UIManager_OnUIInactive(object sender, System.EventArgs e)
    {
        SetGameState(previousState);
    }
    #endregion
}
