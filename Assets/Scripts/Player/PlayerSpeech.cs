using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeech : MonoBehaviour
{
    private AudioSource audioData;
    [SerializeField]
    private PlayerAudioScriptableObject playerAudioData;

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
}
