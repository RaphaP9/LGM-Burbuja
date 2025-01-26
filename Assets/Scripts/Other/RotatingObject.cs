using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        transform.localRotation *= Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
