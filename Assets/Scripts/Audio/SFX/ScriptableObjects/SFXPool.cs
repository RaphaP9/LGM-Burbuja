using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/SFXPool")]
public class SFXPool : ScriptableObject
{
    [Header("Player")]
    public AudioClip[] playerJump;
    public AudioClip[] playerJumpFromBubble;
    [Space]
    public AudioClip[] playerDash;
    public AudioClip[] playerDashFromBubble;
    [Space]
    public AudioClip[] playerLand;
    [Space]
    public AudioClip[] playerBubbleAttach;

    [Header("Others")]
    public AudioClip[] landmark;
}
