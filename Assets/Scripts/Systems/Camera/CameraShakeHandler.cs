using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeHandler : MonoBehaviour
{
    public static CameraShakeHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void Awake()
    {
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
            Debug.LogWarning("There is more than one CameraShakeHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(float amplitude, float frequency, float shakeTime, float fadeInTime, float fadeOutTime)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCameraCoroutine(amplitude, frequency, shakeTime, fadeInTime, fadeOutTime));
    }

    private IEnumerator ShakeCameraCoroutine(float amplitude, float frequency, float shakeTime, float fadeInTime, float fadeOutTime)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;

        float time = 0f;

        while (time <= fadeInTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(0f, amplitude, time / fadeInTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(0f, frequency, time / fadeInTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;

        yield return new WaitForSecondsRealtime(shakeTime);

        time = 0f;

        while (time <= fadeOutTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 0f, time / fadeOutTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 0f, time / fadeOutTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;
    }
}