using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform refference;

    [Header("Settings")]
    [SerializeField] private Vector2 parallaxFactor;

    private Vector3 lastRefferencePosition;

    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        lastRefferencePosition = refference.position;
    }

    void Update()
    {
        HandleParallax();
    }

    private void HandleParallax()
    {
        Vector3 deltaMovement = refference.position - lastRefferencePosition; // Find how much the camera has moved
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y, 0f); // Move the background based on camera movement

        lastRefferencePosition = refference.position;
    }
}
