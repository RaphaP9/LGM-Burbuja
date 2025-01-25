using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBubbleHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerGravityController playerGravityController;

    [Header("Setting")]
    [SerializeField, Range(1f, 100f)] private float smoothAtractionFactor;

    public bool IsOnBubble ;//{ get; private set; }

    public Bubble currentBubble;

    private float previousGravityScale;

    private Rigidbody2D _rigidbody2D;

    private void OnEnable()
    {
        Bubble.OnBubbleEnter += Bubble_OnBubbleEnter;
        Bubble.OnBubbleReleased += Bubble_OnBubbleReleased;
    }
    private void OnDisable()
    {
        Bubble.OnBubbleEnter -= Bubble_OnBubbleEnter;
        Bubble.OnBubbleReleased -= Bubble_OnBubbleReleased;
    }
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleBubbleCenterAtraction();
    }

    private void HandleBubbleCenterAtraction()
    {
        if (currentBubble == null) return;

        transform.position = currentBubble.BubbleCentrer.position;

        //transform.position = Vector3.Lerp(transform.position, currentBubble.BubbleCentrer.position, smoothAtractionFactor * Time.fixedDeltaTime);
    }

    private void SetCurrentBubble(Bubble bubble) => currentBubble = bubble;
    
    private void ClearCurrentBubble() => currentBubble = null;
    private void Bubble_OnBubbleEnter(object sender, Bubble.OnBubbleEventArgs e)
    {
        previousGravityScale = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;
        SetCurrentBubble(e.bubble);
        IsOnBubble = true;
    }

    private void Bubble_OnBubbleReleased(object sender, Bubble.OnBubbleEventArgs e)
    {
        if (currentBubble != e.bubble) return;

        _rigidbody2D.gravityScale = previousGravityScale;
        ClearCurrentBubble();
        IsOnBubble = false;
    }
}
