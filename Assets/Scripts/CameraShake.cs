using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    public CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private CinemachineCamera cinemachineCamera;
    private float shakeTime = 0;
    private float strength = 0;

    private bool isShaking = false;

    private void Awake()
    {
        Instance = this;
        cinemachineCamera = GetComponent<CinemachineCamera>();

        //example shake: CameraShake.Instance.ShakeCamera(5f, 0.1f);
    }

    public void ShakeCamera(float intensity, float time)
    {
        Debug.Log("shake");

        if (isShaking)
        {
            StopCoroutine("ShakeTimer");
        }

        shakeTime = time;
        strength = intensity;

        StartCoroutine(ShakeTimer());
        isShaking = true;
    }

    private IEnumerator ShakeTimer()
    {
        float shakeCountdown = shakeTime;

        while (shakeCountdown > 0)
        {
            shakeCountdown -= Time.deltaTime;

            float shake = Mathf.Lerp(strength, 0f, shakeCountdown / shakeTime);
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = shake;

            yield return null;
        }

        shakeTime = 0;
        isShaking = false;

        cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
    }

    private IEnumerator FreezeFrames(float amount)
    {

        Time.timeScale = 0.1f;

        yield return new WaitForSeconds(0.01f);

        Time.timeScale = 1f;
    }
}