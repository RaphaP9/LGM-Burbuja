using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CapsuleCollider2D capsuleCollider2D;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("General Settings")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Wall Detection Settings")]
    [SerializeField, Range(-0.2f, 1f)] private float wallRayLength = 0.1f;
    [Space]
    [SerializeField, Range(0f, 1f)] private List<float> wallDetectionPoints;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private float MoveDirection => playerMovement.LastNonZeroInput;

    public bool HitWall;// { get; private set; }

    private void FixedUpdate()
    {
        HitWall = CheckIfWall();
    }

    private bool CheckIfWall()
    {
        foreach (float wallDetectionPoint in wallDetectionPoints)
        {
            if (CheckIfWallAtPoint(GeneralMethods.TransformPositionVector2(transform) + new Vector2(0f, capsuleCollider2D.size.y * wallDetectionPoint), wallRayLength)) return true;
        }

        return false;
    }

    private bool CheckIfWallAtPoint(Vector2 origin, float rayLength)
    {
        bool hitWall = false;

        if (MoveDirection != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * MoveDirection, rayLength, obstacleLayer);
            hitWall = hit.collider != null;
        }

        if (drawRaycasts) Debug.DrawRay(origin, Vector2.right * MoveDirection, Color.blue);

        return hitWall;
    }
}
