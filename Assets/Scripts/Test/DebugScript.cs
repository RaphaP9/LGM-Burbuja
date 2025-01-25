using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    private void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }
    private void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        Debug.Log("Resume");
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Debug.Log("Pause");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
