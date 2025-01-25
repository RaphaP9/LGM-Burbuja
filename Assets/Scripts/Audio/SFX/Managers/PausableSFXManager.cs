using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableSFXManager : SFXManager
{
    public static PausableSFXManager Instance { get; private set; }

    private void OnEnable()
    {
        DebugScript.OnPlaySampleAudio += DebugScript_OnPlaySampleAudio;
    }

    private void OnDisable()
    {
        DebugScript.OnPlaySampleAudio -= DebugScript_OnPlaySampleAudio;
    }

    protected override void Awake()
    {
        base.Awake();
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

    private void DebugScript_OnPlaySampleAudio(object sender, EventArgs e)
    {
        PlaySound(SFXPool.bubbleExplode);
    }
}
