using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeech : MonoBehaviour
{
    private AudioSource audioData;
    [SerializeField]
    private AudioClip spawnSound, flashLightCollect, boxNoFlashlight, startFirstLevel;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
        if(UIManager.Instance.isLevelTutorial()) {
            PlaySpawnSound();
        }
    }

    public void PlaySpawnSound() {
        audioData.PlayOneShot(spawnSound, 1f);
    }

    public void PlayFlashlightCollectSound() {
        audioData.PlayOneShot(flashLightCollect, 1f);
    }

    public void PlayBoxNoFlashlightSound() {
        audioData.PlayOneShot(boxNoFlashlight, 1f);
    }

    public void PlayStartLevelSound() {
        audioData.PlayOneShot(startFirstLevel, 1f);
    }
}
