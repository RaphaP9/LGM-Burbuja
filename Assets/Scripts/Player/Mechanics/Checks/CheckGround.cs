using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckGround : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CapsuleCollider2D capsuleCollider2D;

    [Header("Check Ground Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Range(-1f, 1f)] private float checkGroundYOffset = 0.1f;
    [SerializeField, Range(0.01f, 1f)] private float raySphereRadius = 0.1f;

    [Header("Check Slope Settings")]
    [SerializeField, Range(0f, 1f)] private float checkSlopeRayLength = 0.2f;

    [Header("Distance From Ground Settings")]
    [SerializeField, Range(0f, 100f)] private float distanceFromGroundRayLength;
    private Vector2 checkDistanceFromGroundOffset = new Vector2(0f, 0.1f);

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    public bool IsGrounded;// { get; private set; } = false;
    public bool OnSlope; // { get; private set; } = false;
    public Vector2 SlopeNormal;// { get; private set; }
    public float DistanceFromGround;// { get; private set; }

    private void FixedUpdate()
    {
        IsGrounded = CheckGrounded();
        OnSlope = CheckSlope();
        DistanceFromGround = CalculateDistanceFromGround();
    }

    private bool CheckGrounded()
    {
        Vector2 origin = GeneralMethods.TransformPositionVector2(transform) + new Vector2(0f, checkGroundYOffset);

        bool isGrounded = Physics2D.OverlapCircle(origin, raySphereRadius, groundLayer);

        return isGrounded;
    }

    private bool CheckSlope()
    {
        Vector2 origin = GeneralMethods.TransformPositionVector2(transform) + capsuleCollider2D.offset;
        float finalRayLength = checkSlopeRayLength + capsuleCollider2D.offset.y;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, finalRayLength, groundLayer);

        if (hit.collider == null) return false;   

        SlopeNormal = hit.normal;

        if (SlopeNormal == Vector2.up) return false;

        if (drawRaycasts) Debug.DrawRay(origin, Vector2.down * (finalRayLength), Color.cyan);

        return true;
    }

    private float CalculateDistanceFromGround()
    {
        Vector2 origin = GeneralMethods.TransformPositionVector2(transform) + checkDistanceFromGroundOffset;
        float distance = float.MaxValue;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distanceFromGroundRayLength, groundLayer);

        if (hit.collider == null) return distance;

        distance = (hit.point - GeneralMethods.TransformPositionVector2(transform)).magnitude;

        return distance;
    }
}
