using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform bubbleCenter;

    private const string PLAYER_TAG = "Player";

    public static event EventHandler<OnBubbleEventArgs> OnBubbleEnter;
    public static event EventHandler<OnBubbleEventArgs> OnBubbleReleased;

    public Transform BubbleCentrer => bubbleCenter;

    public class OnBubbleEventArgs : EventArgs
    {
        public Bubble bubble;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ReleaseBubble();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        OnBubbleEnter?.Invoke(this, new OnBubbleEventArgs { bubble = this });
    }

    private void ReleaseBubble()
    {
        OnBubbleReleased?.Invoke(this, new OnBubbleEventArgs { bubble = this });
    }
}
