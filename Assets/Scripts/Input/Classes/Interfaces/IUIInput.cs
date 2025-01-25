using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIInput
{
    public bool CanProcessInput();
    public bool GetPauseDown();
}
