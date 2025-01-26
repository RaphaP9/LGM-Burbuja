using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraShakeListener;

public class AbilitiesUnlockListener : MonoBehaviour
{
    [Header("Abilities Unlock Logs")]
    [SerializeField] private string dashUnlockLog;
    [SerializeField] private string doubleJumpUnlockLog;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckUnlockAbilities(string log)
    {
        if (dashUnlockLog == log) PlayerDash.Instance.UnlockDash();
        if (doubleJumpUnlockLog == log) PlayerJump.Instance.UnlockDoubleJump();
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckUnlockAbilities(e.gameplayAction.log);
    }
}
