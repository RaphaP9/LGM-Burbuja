using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform bubbleCenter;

    [Header("Setting")]
    [SerializeField,Range(1f,100f)] private float smoothInFactor;

    private const string PLAYER_TAG = "Player";

    public static event EventHandler<OnBubbleEventArgs> OnBubbleEnter;
    public static event EventHandler<OnBubbleEventArgs> OnBubbleExit;

    public class OnBubbleEventArgs : EventArgs
    {
        public Bubble bubble;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        OnBubbleEnter?.Invoke(this, new OnBubbleEventArgs { bubble = this });
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        OnBubbleExit?.Invoke(this, new OnBubbleEventArgs { bubble = this });
    }
}
