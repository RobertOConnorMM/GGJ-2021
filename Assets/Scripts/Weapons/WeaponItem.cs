using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponItem : MonoBehaviour
{
  [SerializeField]
  private int id = 0;
  [SerializeField]
  private new string name = "Umbrella";
  [SerializeField]
  private float power = 1;
  [SerializeField]
  private float durability = 1;
  private PlayerManager playerManager;
  private bool isUiVisible;

  void Start()
  {
    playerManager = FindObjectOfType<PlayerManager>();
    playerManager.GetActions().Player.Action.performed += OnAction;
  }

  public void OnHit(float damage)
  {
    durability -= damage;

    if (durability < 1)
    {
      Destroy(gameObject);
    }
  }

  // Update is called once per frame
  private void OnTriggerEnter(Collider collision)
  {
    if (collision.gameObject.tag != "Player")
    {
      return;
    }

    ShowUI();
  }

  private void OnTriggerExit(Collider collision)
  {
    if (collision.gameObject.tag != "Player")
    {
      return;
    }

    HideUI();
  }

  private void OnAction(InputAction.CallbackContext context)
  {
    if (isUiVisible)
    {
      var playerCombat = playerManager.GetComponentInChildren<PlayerCombat>();
      playerCombat.GrabItem(gameObject);
    }
  }

  private void ShowUI()
  {
    isUiVisible = true;
  }

  private void HideUI()
  {
    isUiVisible = false;

  }
}
