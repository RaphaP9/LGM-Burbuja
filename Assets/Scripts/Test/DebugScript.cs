using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public static event EventHandler OnPlaySampleAudio;

    private void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;

        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
    }

    private void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;

        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;
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
        CheckPlaySampleAudio();
    }

    private void CheckPlaySampleAudio()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnPlaySampleAudio?.Invoke(this,EventArgs.Empty);
        }
    }

    private void PlayerJump_OnPlayerJump(object sender, EventArgs e)
    {
        Debug.Log("Jump");
    }
    private void PlayerFall_OnPlayerFall(object sender, EventArgs e)
    {
        Debug.Log("Fall");
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        Debug.Log("Land");
    }

}
