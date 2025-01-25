using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPauseHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<AudioSource> audioSourcesToPause;

    [Header("Enabler")]
    [SerializeField] private bool enable;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    private void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    private void PauseAudioSources()
    {
        foreach (AudioSource audioSource in audioSourcesToPause)
        {
            audioSource.Pause();
        }
    }

    private void UnPauseAudioSources()
    {
        foreach (AudioSource audioSource in audioSourcesToPause)
        {
            audioSource.UnPause();
        }
    }


    private void PauseGlobalSFX()
    {
        if (!enable) return;
        PauseAudioSources();
    }

    private void ResumeGlobalSFX()
    {
        if (!enable) return;
        UnPauseAudioSources();
    }

    private void PauseAllTemporalSFX()
    {
        if (!enable) return;

        TemporalSFXController[] temporalSFXControllers = FindObjectsOfType<TemporalSFXController>();

        foreach (TemporalSFXController temporalSFXController in temporalSFXControllers)
        {
            if (!temporalSFXController.Pausable) continue;

            AudioSource audioSource = temporalSFXController.GetComponent<AudioSource>();

            if (!audioSource)
            {
                if (debug) Debug.LogWarning("TemporalSFX does not have an audiosource component");
                continue;
            }

            audioSource.Pause();
        }
    }

    private void ResumeAllTemporalSFX()
    {
        if (!enable) return;

        TemporalSFXController[] temporalSFXControllers = FindObjectsOfType<TemporalSFXController>();

        foreach (TemporalSFXController temporalSFXController in temporalSFXControllers)
        {
            if (!temporalSFXController.Pausable) continue;

            AudioSource audioSource = temporalSFXController.GetComponent<AudioSource>();

            if (!audioSource)
            {
                if (debug) Debug.LogWarning("TemporalSFX does not have an audiosource component");
                continue;
            }

            audioSource.UnPause();
        }
    }
    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        PauseGlobalSFX();
        PauseAllTemporalSFX();
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        ResumeGlobalSFX();
        ResumeAllTemporalSFX();
    }
}
