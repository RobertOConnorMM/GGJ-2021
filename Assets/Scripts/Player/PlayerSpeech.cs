using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeech : MonoBehaviour
{
    private AudioSource audioData;
    [SerializeField]
    private PlayerAudioScriptableObject playerAudioData;
    private int lastPlayedStep = 0;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
        if(UIManager.Instance.isLevelTutorial()) {
            PlaySpawnSound();
        }
    }

    public void PlaySpawnSound() {
        audioData.PlayOneShot(playerAudioData.spawnSound, 1f);
    }

    public void PlayFlashlightCollectSound() {
        audioData.PlayOneShot(playerAudioData.flashLightCollect, 1f);
    }

    public void PlayBoxNoFlashlightSound() {
        audioData.PlayOneShot(playerAudioData.boxNoFlashlight, 1f);
    }

    public void PlayStartLevelSound() {
        audioData.PlayOneShot(playerAudioData.startFirstLevel, 1f);
    }

    public void PlayThrowSound() {
        audioData.PlayOneShot(playerAudioData.throwSound, 0.6f);
    }

    public void PlayPickupSound() {
        audioData.PlayOneShot(playerAudioData.pickupSound, 0.6f);
    }

    public void PlayFootstep() {
        if(lastPlayedStep == 0) {
            audioData.PlayOneShot(playerAudioData.footstep2, 0.3f);
            lastPlayedStep = 1;
        } else {
            audioData.PlayOneShot(playerAudioData.footstep, 0.3f);
            lastPlayedStep = 0;
        }
    }
}
