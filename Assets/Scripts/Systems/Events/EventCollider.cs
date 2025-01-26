using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCollider : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    [Header("Settings")]
    [SerializeField] protected int eventID;
    [Space]
    [SerializeField] protected float timeToTrigger;
    [SerializeField] protected bool multipleTriggers;
    [SerializeField] protected bool hasBeenTriggered;

    public static event EventHandler<OnEventColliderTriggerEventArgs> OnEventColliderTrigger;

    public class OnEventColliderTriggerEventArgs : EventArgs
    {
        public int eventID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        HandleColliderTrigger();
    }

    protected virtual void HandleColliderTrigger()
    {
        if (hasBeenTriggered && !multipleTriggers) return;

        TriggerCollider();
        hasBeenTriggered = true;
    }

    protected virtual void TriggerCollider()
    {
        StartCoroutine(TriggerColliderCoroutine());
    }

    private IEnumerator TriggerColliderCoroutine()
    {
        yield return new WaitForSeconds(timeToTrigger);
        OnEventColliderTrigger?.Invoke(this, new OnEventColliderTriggerEventArgs { eventID = eventID });
    }
}
