using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
  public PlayerManager playerManager;
  public float minSpeed = 4f;
  public float maxSpeed = 15f;
  public float maxChargeTime = .6f;
  private float chargeTimeStart;
  private bool nearBox;
  private LostAndFoundBox box;
  [SerializeField] WeaponScriptableObject weaponScriptableObject;
  private GameObject grabbedItem;

  void Start()
  {
    playerManager.GetActions().Player.Action.performed += OnAction;
    playerManager.GetActions().Player.Fire.started += OnFireStart;
    playerManager.GetActions().Player.Fire.canceled += OnFireCanceled;
  }

  public void GrabItem(GameObject gameObject)
  {
    ReleaseItem();

    var rigidBody = gameObject.GetComponent<Rigidbody>();
    rigidBody.isKinematic = true;

    grabbedItem = gameObject;
    grabbedItem.transform.parent = playerManager.rightHand;
    grabbedItem.transform.localPosition = Vector3.zero;
  }

  private void ReleaseItem()
  {
    if (grabbedItem == null)
    {
      return;
    }

    var rigidBody = grabbedItem.GetComponent<Rigidbody>();
    if (rigidBody != null)
    {
      rigidBody.isKinematic = false;
    }

    // Detach grabbed item, enable rigidbody and add force to "shoot" it
    grabbedItem.transform.parent = null;
    grabbedItem = null;
  }

  private void OnAction(InputAction.CallbackContext context)
  {
    if (box == null || (!nearBox && grabbedItem != null && !box.hasItems()))
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

    GrabItem(Instantiate(gameObjectPrefab));

    box.HideUI();
  }

  public void OnNearBox(LostAndFoundBox _box)
  {
    nearBox = true;
    box = _box;
  }

  public void OnLeaveBox()
  {
    nearBox = false;
    box = null;
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


    var rigidBody = grabbedItem.GetComponent<Rigidbody>();
    rigidBody.isKinematic = false;
    rigidBody.AddTorque(
        Quaternion.Lerp(
            Random.rotation,
            Quaternion.Euler(playerManager.rightHand.transform.forward),
            .9f
        ).eulerAngles * speed,
        ForceMode.Impulse
    );
    rigidBody.AddForce(
      playerManager.rightHand.transform.forward * speed,
      ForceMode.Impulse
    );


    ReleaseItem();
  }
}