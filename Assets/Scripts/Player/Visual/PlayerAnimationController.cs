using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerBubbleHandler playerBubbleHandler;

    private const string JUMP_TRIGGER = "Jump";
    private const string FALL_TRIGGER = "Fall";
    private const string LAND_TRIGGER = "Land";
    private const string DASH_TRIGGER = "Dash";
    private const string BUBBLE_ATTACH_TRIGGER = "BubbleAttach";

    private const string SPEED_FLOAT = "Speed";

    private const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";
    private const string JUMP_ANIMATION_NAME = "Jump";
    private const string FALL_ANIMATION_NAME = "Fall";
    private const string DASH_ANIMATION_NAME = "Dash";
    private const string BUBBLE_ATTACH_ANIMATION_NAME = "BubbleAttach";


    private void OnEnable()
    {
        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;

        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped += PlayerDash_OnPlayerDashStopped;
        PlayerBubbleHandler.OnBubbleAttach += PlayerBubbleHandler_OnBubbleAttach;
    }

    private void OnDisable()
    {
        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;

        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped -= PlayerDash_OnPlayerDashStopped;
        PlayerBubbleHandler.OnBubbleAttach -= PlayerBubbleHandler_OnBubbleAttach;
    }

    private void Update()
    {
        HandleSpeedBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, playerMovement.DesiredSpeed);
    }

    private void ResetTriggers()
    {
        animator.ResetTrigger(JUMP_TRIGGER);
        animator.ResetTrigger(FALL_TRIGGER);
        animator.ResetTrigger(LAND_TRIGGER);
        animator.ResetTrigger(DASH_TRIGGER);
        animator.ResetTrigger(BUBBLE_ATTACH_TRIGGER);
    }


    private void PlayerJump_OnPlayerJump(object sender, System.EventArgs e)
    {
        ResetTriggers();
        animator.Play(JUMP_ANIMATION_NAME);
        Debug.Log("Jump");
    }
    private void PlayerFall_OnPlayerFall(object sender, System.EventArgs e)
    {
        ResetTriggers();
        animator.Play(FALL_ANIMATION_NAME);
        Debug.Log("Fall");
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        if (playerBubbleHandler.IsOnBubble) return;

        ResetTriggers();
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
        Debug.Log("Land");
    }
    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        ResetTriggers();
        animator.Play(DASH_ANIMATION_NAME);
        Debug.Log("Dash");
    }

    private void PlayerDash_OnPlayerDashStopped(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        ResetTriggers();
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
        Debug.Log("DashStopped");
    }

    private void PlayerBubbleHandler_OnBubbleAttach(object sender, System.EventArgs e)
    {
        ResetTriggers();
        animator.Play(BUBBLE_ATTACH_ANIMATION_NAME);
        Debug.Log("BubbleAttach");
    }
}
