using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAnimationsController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Bubble bubble;

    private const string IDLE_ANIMATION_NAME = "Idle";
    private const string INSIDE_ANIMATION_NAME = "Inside";

    private void OnEnable()
    {
        PlayerBubbleHandler.OnBubbleAttach += PlayerBubbleHandler_OnBubbleAttach;
        PlayerBubbleHandler.OnBubbleUnattach += PlayerBubbleHandler_OnBubbleUnattach;
    }

    private void OnDisable()
    {
        PlayerBubbleHandler.OnBubbleAttach -= PlayerBubbleHandler_OnBubbleAttach;
        PlayerBubbleHandler.OnBubbleUnattach -= PlayerBubbleHandler_OnBubbleUnattach;
    }
    private void PlayerBubbleHandler_OnBubbleAttach(object sender, PlayerBubbleHandler.OnBubbleEventArgs e)
    {
        if (e.bubble != bubble) return;

        animator.Play(INSIDE_ANIMATION_NAME);
    }

    private void PlayerBubbleHandler_OnBubbleUnattach(object sender, PlayerBubbleHandler.OnBubbleEventArgs e)
    {
        if (e.bubble != bubble) return;

        animator.Play(IDLE_ANIMATION_NAME);

    }

}
