using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventCollider.OnEventColliderTrigger += EventCollider_OnEventColliderTrigger;
        Landmark.OnLandmarkTriggered += Landmark_OnLandmarkTriggered;
        Landmark.OnLandmarkTriggeredMessage += Landmark_OnLandmarkTriggeredMessage;
        Landmark.OnLandmarkTriggeredEnd += Landmark_OnLandmarkTriggeredEnd;

        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
    }

    private void OnDisable()
    {
        EventCollider.OnEventColliderTrigger -= EventCollider_OnEventColliderTrigger;
        Landmark.OnLandmarkTriggered -= Landmark_OnLandmarkTriggered;

        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
    }

    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        if (e.fromBubble)
        {
            GameLogManager.Instance.Log($"Movement/Dash/FromBubble");
        }
        else
        {
            GameLogManager.Instance.Log($"Movement/Dash/Regular");
        }
    }

    private void PlayerJump_OnPlayerJump(object sender, PlayerJump.OnPlayerJumpEventArgs e)
    {
        if (e.fromBubble)
        {
            GameLogManager.Instance.Log($"Movement/Jump/FromBubble");
        }
        else
        {
            GameLogManager.Instance.Log($"Movement/Jump/Regular");
        }
    }

    private void EventCollider_OnEventColliderTrigger(object sender, EventCollider.OnEventColliderTriggerEventArgs e)
    {
        GameLogManager.Instance.Log($"Events/EventCollider/{e.eventID}");
    }
    private void Landmark_OnLandmarkTriggered(object sender, Landmark.OnLandmarkEventArgs e)
    {
        GameLogManager.Instance.Log($"Landmark/{e.id}");

    }
    private void Landmark_OnLandmarkTriggeredEnd(object sender, Landmark.OnLandmarkEventArgs e)
    {
        GameLogManager.Instance.Log($"LandmarkEnd/{e.id}");
    }

    private void Landmark_OnLandmarkTriggeredMessage(object sender, Landmark.OnLandmarkEventArgs e)
    {
        GameLogManager.Instance.Log($"LandmarkMessage/{e.id}");
    }
}
