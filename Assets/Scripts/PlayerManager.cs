using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public PlayerInputActions playerActions;

  void Awake() {
    playerActions = new PlayerInputActions();
  }

  public PlayerInputActions GetActions() {
    return playerActions;
  }

  private void OnEnable()
  {
    playerActions.Enable();
  }

  private void OnDisable()
  {
    playerActions.Disable();
  }

}
