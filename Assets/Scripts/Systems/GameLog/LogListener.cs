using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }

    private void SampleLog()
    {
        GameLogManager.Instance.Log("Sample/Log");
    }
}
