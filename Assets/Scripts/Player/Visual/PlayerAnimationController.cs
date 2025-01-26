using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    private const string JUMP_TRIGGER = "Jump";
    private const string FALL_TRIGGER = "Jump";
    private const string LAND_TRIGGER = "Land";
    private const string DASH_TRIGGER = "Dash";
    private const string DASH_END_TRIGGER = "DashEnd";


    private void OnEnable()
    {
        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped += PlayerDash_OnPlayerDashStopped;
    }

    private void OnDisable()
    {
        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;

        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped -= PlayerDash_OnPlayerDashStopped;
    }


    private void PlayerJump_OnPlayerJump(object sender, System.EventArgs e)
    {
        animator.SetTrigger(JUMP_TRIGGER);
    }
    private void PlayerFall_OnPlayerFall(object sender, System.EventArgs e)
    {
        animator.SetTrigger(FALL_TRIGGER);
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        animator.SetTrigger(LAND_TRIGGER);

    }
    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        animator.SetTrigger(DASH_TRIGGER);
    }

    private void PlayerDash_OnPlayerDashStopped(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        animator.SetTrigger(DASH_END_TRIGGER);
    }

}
