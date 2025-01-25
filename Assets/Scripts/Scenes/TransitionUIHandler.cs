using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator transitionUIAnimator;

    private const string FADE_OUT_TRIGGER = "FadeOut";
    private const string FADE_IN_TRIGGER = "FadeIn";

    public static event EventHandler OnFadeOutEnd;
    public static event EventHandler OnFadeInEnd;

    private void OnEnable()
    {
        ScenesManager.OnSceneTransitionOutStart += ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart += ScenesManager_OnSceneTransitionInStart;
    }

    private void OnDisable()
    {
        ScenesManager.OnSceneTransitionOutStart -= ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart -= ScenesManager_OnSceneTransitionInStart;
    }

    private void TriggerFadeOut()
    {
        transitionUIAnimator.ResetTrigger(FADE_IN_TRIGGER);
        transitionUIAnimator.SetTrigger(FADE_OUT_TRIGGER);
    }
    private void TriggerFadeIn()
    {
        transitionUIAnimator.ResetTrigger(FADE_OUT_TRIGGER);
        transitionUIAnimator.SetTrigger(FADE_IN_TRIGGER);
    }

    public void FadeInEnd() => OnFadeInEnd?.Invoke(this, EventArgs.Empty);
    public void FadeOutEnd() => OnFadeOutEnd?.Invoke(this, EventArgs.Empty);


    #region ScenesManager Subscriptions
    private void ScenesManager_OnSceneTransitionOutStart(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        TriggerFadeOut();
    }

    private void ScenesManager_OnSceneTransitionInStart(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        TriggerFadeIn();
    }
    #endregion
}