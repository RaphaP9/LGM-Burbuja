using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    public enum State { Idle, TransitionOut, FullBlack, TransitionIn }

    public State SceneState => state;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 0.5f)] private float fadeInInterval;

    public static event EventHandler<OnSceneLoadEventArgs> OnSceneTransitionOutStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneTransitionInStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoadStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoad;

    private string sceneToLoad;

    public class OnSceneLoadEventArgs : EventArgs
    {
        public string sceneName;
    }

    private void OnEnable()
    {
        TransitionUIHandler.OnFadeOutEnd += TransitionUIHandler_OnFadeOutEnd;
        TransitionUIHandler.OnFadeInEnd += TransitionUIHandler_OnFadeInEnd;
    }

    private void OnDisable()
    {
        TransitionUIHandler.OnFadeOutEnd -= TransitionUIHandler_OnFadeOutEnd;
        TransitionUIHandler.OnFadeInEnd -= TransitionUIHandler_OnFadeInEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { sceneName = SceneManager.GetActiveScene().name });
        OnSceneTransitionInStart.Invoke(this, new OnSceneLoadEventArgs { sceneName = SceneManager.GetActiveScene().name });
        SetSceneState(State.TransitionIn);
    }

    private void SetSceneState(State state) => this.state = state;

    private bool CanChangeScene() => state == State.Idle;

    #region SimpleLoad
    public void SimpleReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SimpleLoadTargetScene(currentSceneName);
    }

    public void SimpleLoadTargetScene(string sceneName)
    {
        if (!CanChangeScene()) return;
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    #endregion

    #region FadeLoad

    public void FadeLoadTargetScene(string sceneName)
    {
        if (!CanChangeScene()) return;

        SetSceneState(State.TransitionOut);
        OnSceneTransitionOutStart?.Invoke(this, new OnSceneLoadEventArgs { sceneName = sceneName });
        sceneToLoad = sceneName;
    }

    public void FadeReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        FadeLoadTargetScene(currentSceneName);
    }

    private IEnumerator LoadSceneTransitionCoroutine(string sceneName)
    {
        yield return StartCoroutine(LoadSceneCoroutine(sceneName));

        yield return new WaitForSeconds(0.1f);
        OnSceneTransitionInStart?.Invoke(this, new OnSceneLoadEventArgs { sceneName = sceneName });

        SetSceneState(State.TransitionIn);

        sceneToLoad = "";
    }

    #endregion

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { sceneName = sceneName });
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        OnSceneLoadStart?.Invoke(this, new OnSceneLoadEventArgs { sceneName = sceneName });

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        OnSceneLoad?.Invoke(this, new OnSceneLoadEventArgs { sceneName = sceneName });
    }

    public void QuitGame() => Application.Quit();

    #region TransitionUIHandler Subscriptions
    private void TransitionUIHandler_OnFadeOutEnd(object sender, EventArgs e)
    {
        SetSceneState(State.FullBlack);
        StartCoroutine(LoadSceneTransitionCoroutine(sceneToLoad));
    }
    private void TransitionUIHandler_OnFadeInEnd(object sender, EventArgs e)
    {
        SetSceneState(State.Idle);
    }
    #endregion

}

