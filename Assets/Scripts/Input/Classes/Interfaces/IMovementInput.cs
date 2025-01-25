using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementInput
{
    public bool CanProcessInput();
    public float GetMovementInputNormalized();
}
