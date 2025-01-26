using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableSFXManager : SFXManager
{
    public static PausableSFXManager Instance { get; private set; }

    private void OnEnable()
    {
        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
        PlayerBubbleHandler.OnBubbleAttach += PlayerBubbleHandler_OnBubbleAttach;
    }

    private void OnDisable()
    {
        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;
        PlayerBubbleHandler.OnBubbleAttach -= PlayerBubbleHandler_OnBubbleAttach;
    }

    #region Singleton Settings
    protected override void Awake()
    {
        base.Awake();
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one AudioManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    #endregion

    private void PlayerJump_OnPlayerJump(object sender, PlayerJump.OnPlayerJumpEventArgs e)
    {
        if (e.fromBubble)
        {
            PlaySound(SFXPool.playerJumpFromBubble);
        }
        else
        {
            PlaySound(SFXPool.playerJump);
        }
    }
    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        if (e.fromBubble)
        {
            PlaySound(SFXPool.playerDashFromBubble);
        }
        else
        {
            PlaySound(SFXPool.playerDash);
        }
    }
    private void PlayerBubbleHandler_OnBubbleAttach(object sender, EventArgs e)
    {
        PlaySound(SFXPool.playerBubbleAttach);
    }
    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        PlaySound(SFXPool.playerLand);
    }
}
