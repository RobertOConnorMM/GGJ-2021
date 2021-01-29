using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputActions;

public class Player : MonoBehaviour
{
  public PlayerActions PlayerActions = new PlayerActions();

  private void OnEnable()
  {
    PlayerActions.Enable();
  }

  private void OnDisable()
  {
    PlayerActions.Disable();
  }

}
