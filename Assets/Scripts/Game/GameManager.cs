using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    public enum State { OnGameplay, OnUI, OnLandmark, OnWin}

    public State GameState => state;

    private void OnEnable()
    {
        UIManager.OnUIActive += UIManager_OnUIActive;
        UIManager.OnUIInactive += UIManager_OnUIInactive;

        Landmark.OnLandmarkTriggered += Landmark_OnLandmarkTriggered;
        Landmark.OnLandmarkTriggeredEnd += Landmark_OnLandmarkTriggeredEnd;

        WinManager.OnWin += WinManager_OnWin;
    }

    private void OnDisable()
    {
        UIManager.OnUIActive -= UIManager_OnUIActive;
        UIManager.OnUIInactive -= UIManager_OnUIInactive;

        Landmark.OnLandmarkTriggered -= Landmark_OnLandmarkTriggered;
        Landmark.OnLandmarkTriggeredEnd -= Landmark_OnLandmarkTriggeredEnd;

        WinManager.OnWin -= WinManager_OnWin;
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
        SetPreviousState(this.state);
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

    #region Landmark Subscriptions
    private void Landmark_OnLandmarkTriggered(object sender, Landmark.OnLandmarkEventArgs e)
    {
        SetGameState(State.OnLandmark);
    }

    private void Landmark_OnLandmarkTriggeredEnd(object sender, Landmark.OnLandmarkEventArgs e)
    {
        SetGameState(State.OnGameplay);
    }
    #endregion

    #region WinManager Subsciptions
    private void WinManager_OnWin(object sender, System.EventArgs e)
    {
        SetGameState(State.OnWin);

    }
    #endregion
}
