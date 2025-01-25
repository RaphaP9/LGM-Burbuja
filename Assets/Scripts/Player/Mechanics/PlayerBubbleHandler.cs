using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBubbleHandler : MonoBehaviour
{
    public bool IsOnBubble { get; private set; }

    private Bubble currentBubble;

    private void OnEnable()
    {
        Bubble.OnBubbleEnter += Bubble_OnBubbleEnter;
        Bubble.OnBubbleExit += Bubble_OnBubbleExit;
    }
    private void OnDisable()
    {
        Bubble.OnBubbleEnter -= Bubble_OnBubbleEnter;
        Bubble.OnBubbleExit -= Bubble_OnBubbleExit;
    }


    private void SetCurrentBubble(Bubble bubble) => currentBubble = bubble;
    
    private void ClearCurrentBubble() => currentBubble = null;
    private void Bubble_OnBubbleEnter(object sender, Bubble.OnBubbleEventArgs e)
    {
        SetCurrentBubble(e.bubble);
    }

    private void Bubble_OnBubbleExit(object sender, Bubble.OnBubbleEventArgs e)
    {
        ClearCurrentBubble();
    }
}
