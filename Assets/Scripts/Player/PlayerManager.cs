using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public PlayerInputActions playerActions;

  public GameObject character;
  public Transform leftHand;
  public Transform rightHand;

  public GameObject flashlight;

  void Awake()
  {
    playerActions = new PlayerInputActions();
  }

  public PlayerInputActions GetActions()
  {
    return playerActions;
  }

  public GameObject GetCharacter()
  {
    return character;
  }

  private void OnEnable()
  {
    playerActions?.Enable();
  }

  private void OnDisable()
  {
    playerActions?.Disable();
  }

  public Transform getTransform() {
    return transform;
  }

  public void SetFlashlightOn(bool isOn) {
    flashlight.SetActive(isOn);
    UIManager.Instance.OnColletFlashLight();
    GetComponent<PlayerSpeech>().PlayFlashlightCollectSound();
    GetComponent<PlayerSpeech>().PlayPickupSound();
  }
}
