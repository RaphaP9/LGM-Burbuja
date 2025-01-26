using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkPersonHandler : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 11);
    }
}
