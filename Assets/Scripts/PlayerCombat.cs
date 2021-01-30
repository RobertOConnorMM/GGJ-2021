using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
  public float minSpeed = 4f;
  public float maxSpeed = 15f;
  public float maxChargeTime = .6f;
  private float chargeTimeStart;
  private bool nearBox;
  private LostAndFoundBox box;
  [SerializeField] WeaponScriptableObject weaponScriptableObject;
  private PlayerManager player;
  private GameObject grabbedItem;

  void Start()
  {
    player = GetComponent<PlayerManager>();
    player.GetActions().Player.Action.performed += OnAction;
    player.GetActions().Player.Fire.started += OnFireStart;
    player.GetActions().Player.Fire.canceled += OnFireCanceled;
  }

  private void OnAction(InputAction.CallbackContext context)
  {
    if (!nearBox && grabbedItem != null && !box.hasItems())
    {
      return;
    }

    int newItemId = box.TakeItem();

    GameObject gameObjectPrefab;
    if (newItemId == WeaponIDs.UMBRELLA)
    {
      gameObjectPrefab = weaponScriptableObject.umbrella;
    }
    else if (newItemId == WeaponIDs.TEDDY)
    {
      gameObjectPrefab = weaponScriptableObject.teddy;
    }
    else if (newItemId == WeaponIDs.BOOK)
    {
      gameObjectPrefab = weaponScriptableObject.book;
    }
    else if (newItemId == WeaponIDs.WALLET)
    {
      gameObjectPrefab = weaponScriptableObject.wallet;
    }
    else if (newItemId == WeaponIDs.IPAD)
    {
      gameObjectPrefab = weaponScriptableObject.ipad;
    }
    else
    {
      gameObjectPrefab = weaponScriptableObject.ipad;
    }

    grabbedItem = Instantiate(gameObjectPrefab);
    grabbedItem.transform.parent = player.rightHand;

    var rigidBody = grabbedItem.GetComponent<Rigidbody>();
    rigidBody.isKinematic = true;

    box.HideUI();
  }

  public void OnNearBox(LostAndFoundBox box)
  {
    nearBox = true;
    this.box = box;
  }

  public void OnLeaveBox()
  {
    box = null;
    nearBox = false;
  }

  private void OnFireStart(InputAction.CallbackContext context)
  {
    if (grabbedItem == null)
    {
      return;
    }

    chargeTimeStart = Time.realtimeSinceStartup;
  }

  private void OnFireCanceled(InputAction.CallbackContext context)
  {
    if (grabbedItem == null)
    {
      return;
    }

    var chargeAmount = Mathf.Min(
        maxChargeTime,
        (Time.realtimeSinceStartup - chargeTimeStart) / maxChargeTime
    );

    var speed = ((maxSpeed - minSpeed) * chargeAmount) + minSpeed;

    // Detach grabbed item, enable rigidbody and add force to "shoot" it
    grabbedItem.transform.parent = null;

    var rigidBody = grabbedItem.GetComponent<Rigidbody>();
    rigidBody.isKinematic = false;
    rigidBody.AddTorque(
        Quaternion.Lerp(
            Random.rotation,
            Quaternion.Euler(player.rightHand.transform.forward),
            .9f
        ).eulerAngles * speed,
        ForceMode.Impulse
    );
    rigidBody.AddForce(
      player.rightHand.transform.forward * speed,
      ForceMode.Impulse
    );

    grabbedItem = null;
  }
}