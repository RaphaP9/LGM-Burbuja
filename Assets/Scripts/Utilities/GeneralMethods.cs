using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralMethods
{
    #region Vectors
    public static Vector2 SupressZComponent(Vector3 vector3) => new Vector2(vector3.x, vector3.y);
    #endregion+

    #region Transforms
    public static Vector2 TransformPositionVector2(Transform transform) => new Vector2(transform.position.x, transform.position.y);

    public static List<Transform> GetTransformsByColliders(Collider2D[] colliders)
    {
        List<Transform> transforms = new List<Transform>();

        foreach (Collider2D collider in colliders)
        {
            Transform transform = GetTransformByCollider(collider);

            transforms.Add(transform);
        }

        return transforms;
    }

    public static Transform GetTransformByCollider(Collider2D collider) => collider.transform;

    #endregion
}
