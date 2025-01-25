using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpausableSFXManager : SFXManager
{
    public static UnpausableSFXManager Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one AudioManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
