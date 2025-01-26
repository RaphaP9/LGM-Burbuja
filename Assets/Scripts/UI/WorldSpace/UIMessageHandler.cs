using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private string logToShow;
    [SerializeField] private string logToHide;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);  
    }

    private void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void CheckShow(string log)
    {
        if (logToShow != log) return;

        ShowUI();
    }

    private void CheckHide(string log)
    {
        if (logToHide != log) return;

        HideUI();
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckShow(e.gameplayAction.log);
        CheckHide(e.gameplayAction.log);
    }
}
