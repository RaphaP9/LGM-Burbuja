using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Landmark : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int id;
    [SerializeField] private string logToTrigger;
    [SerializeField] private Animator personAnimator;
    [SerializeField] private Animator bubbleAnimator;
    [SerializeField] private Rigidbody2D personRigidbody2D;
    [Space]
    [SerializeField] private float timeToTrigger;
    [SerializeField] private float timeToTriggerMessage;
    [SerializeField] private float timeToTriggerEnd;

    private const string SET_FREE_TRIGGER = "SetFree";

    public static event EventHandler<OnLandmarkEventArgs> OnLandmarkTriggered;
    public static event EventHandler<OnLandmarkEventArgs> OnLandmarkTriggeredMessage;
    public static event EventHandler<OnLandmarkEventArgs> OnLandmarkTriggeredEnd;

    public class OnLandmarkEventArgs
    {
        public int id;
    }

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckSetLandmark(string log)
    {
        if (logToTrigger != log) return;

        StartCoroutine(SetLandmarkCoroutine());
    }

    private IEnumerator SetLandmarkCoroutine()
    {
        OnLandmarkTriggered?.Invoke(this, new OnLandmarkEventArgs { id = id });

        yield return new WaitForSeconds(timeToTrigger);

        if(personAnimator) personAnimator.SetTrigger(SET_FREE_TRIGGER);
        if(bubbleAnimator) bubbleAnimator.SetTrigger(SET_FREE_TRIGGER);
        personRigidbody2D.isKinematic = false;

        yield return new WaitForSeconds(timeToTriggerMessage);

        OnLandmarkTriggeredMessage?.Invoke(this, new OnLandmarkEventArgs { id = id });

        yield return new WaitForSeconds(timeToTriggerEnd);

        OnLandmarkTriggeredEnd?.Invoke(this, new OnLandmarkEventArgs { id = id });
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckSetLandmark(e.gameplayAction.log);
    }
}
