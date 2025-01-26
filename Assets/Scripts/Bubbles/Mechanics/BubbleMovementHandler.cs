using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMovementHandler : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform positionA;
    [SerializeField] private Transform positionB;

    [Header("Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float idleTime;


    [Header("States")]
    [SerializeField] private State state;

    private enum State { IdleA, MovingTowardsB, IdleB, MovingTowardsA}

    private float timer;

    private void Start()
    {
        InitializePosition();
        SetState(State.IdleA);
        ResetTimer();
    }

    private void Update()
    {
        HandleStates();
    }

    private void SetState(State state) => this.state = state;

    private void HandleStates()
    {
        switch (state)
        {
            case State.IdleA:
                IdleALogic();
                break;
            case State.MovingTowardsB:
                MovingTowardsBLogic();
                break;
            case State.IdleB:
                IdleBLogic();
                break;
            case State.MovingTowardsA:
                MovingTowardsALogic();
                break;
        }
    }

    private void InitializePosition()
    {
        transform.position = positionA.position;
    }

    private void IdleALogic()
    {
        if(timer < idleTime)
        {
            timer += Time.deltaTime;
            return;
        }

        ResetTimer();
        SetState(State.MovingTowardsB);
    }
    private void IdleBLogic()
    {
        if (timer < idleTime)
        {
            timer += Time.deltaTime;
            return;
        }

        ResetTimer();
        SetState(State.MovingTowardsA);
    }

    private void MovingTowardsALogic()
    {
        float totalTime = Vector2.Distance(positionA.position, positionB.position)/movementSpeed; 
        
        if(timer < totalTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(positionB.position, positionA.position, moveCurve.Evaluate(timer / totalTime));
            return;
        }
        
        transform.position = positionA.position;
        ResetTimer();
        SetState(State.IdleA);
    }

    private void MovingTowardsBLogic()
    {
        float totalTime = Vector2.Distance(positionA.position, positionB.position) / movementSpeed;

        if (timer < totalTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(positionA.position, positionB.position, moveCurve.Evaluate(timer / totalTime));
            return;
        }

        transform.position = positionB.position;
        ResetTimer();
        SetState(State.IdleB);
    }

    private void ResetTimer() => timer = 0f;
}
