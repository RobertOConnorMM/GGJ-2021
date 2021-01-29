using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera camera;
    private float shakeTimer;

    public void Awake () {
        Instance = this;
        camera = GetComponent<CinemachineVirtualCamera> ();
    }

    public void Shake (float intensity, float time) {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update () {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f) {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}