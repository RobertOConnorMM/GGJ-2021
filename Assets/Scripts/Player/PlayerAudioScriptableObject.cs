using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerAudioScriptableObject", order = 1)]
public class PlayerAudioScriptableObject : ScriptableObject
{
    public AudioClip spawnSound, flashLightCollect, boxNoFlashlight, startFirstLevel, throwSound;
}