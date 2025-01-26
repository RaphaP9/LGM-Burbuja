using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string logToWin;
    [SerializeField] private string transitionScene;
    [SerializeField, Range(1f, 10f)] private float timeToTransitionAfterWin;
    
    public static event EventHandler OnWin;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Win()
    {
        StopAllCoroutines();
        StartCoroutine(WinCoroutine());
    }
    private IEnumerator WinCoroutine()
    {
        OnWin?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(timeToTransitionAfterWin);
        ScenesManager.Instance.FadeLoadTargetScene(transitionScene);
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToWin) return;
        Win();
    }
}
