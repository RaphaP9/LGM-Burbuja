using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    [Header("Camera Shake Objects Settings")]
    [SerializeField] private List<CameraShakeObject> cameraShakeObjects;

    [Serializable]
    public class CameraShakeObject
    {
        public int id;
        public string logToShake;
        public CameraShakeSettings settings;
    }

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckStartShake(string log)
    {
        foreach(CameraShakeObject cameraShakeObject in cameraShakeObjects)
        {
            if(cameraShakeObject.logToShake == log)
            {
                CameraShakeHandler.Instance.ShakeCamera(cameraShakeObject.settings.amplitude, cameraShakeObject.settings.frequency, cameraShakeObject.settings.shakeTime, cameraShakeObject.settings.shakeFadeInTime, cameraShakeObject.settings.shakeFadeOutTime);
                return;
            }
        }
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartShake(e.gameplayAction.log);
    }

    [Serializable]
    public class CameraShakeSettings
    {
        [SerializeField, Range(0, 10f)] public float amplitude;
        [SerializeField, Range(0, 5f)] public float frequency;
        [SerializeField, Range(0.1f, 10f)] public float shakeTime;
        [SerializeField, Range(0f, 10f)] public float shakeFadeInTime;
        [SerializeField, Range(0f, 10f)] public float shakeFadeOutTime;
    }
}