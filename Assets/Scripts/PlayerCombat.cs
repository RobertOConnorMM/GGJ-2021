using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour {
    private bool nearBox = false;
    private bool hasTakenItem = false;
    private LostAndFoundBox box = null;
    [SerializeField] WeaponScriptableObject weaponScriptableObject;
    private PlayerManager player;

    void Start () {
        player = GetComponent<PlayerManager> ();
        player.GetActions ().Player.Action.performed += OnAction;
    }

    private void OnAction (InputAction.CallbackContext context) {
        if (nearBox && !hasTakenItem && box.hasItems ()) {
            int newItemId = box.TakeItem ();
            Vector3 itemPos = transform.position;
            itemPos.y += 3f;

            GameObject gameObjectPrefab;
            if (newItemId == WeaponIDs.UMBRELLA) {
                gameObjectPrefab = weaponScriptableObject.umbrella;
            } else if (newItemId == WeaponIDs.TEDDY) {
                gameObjectPrefab = weaponScriptableObject.teddy;
            } else if (newItemId == WeaponIDs.BOOK) {
                gameObjectPrefab = weaponScriptableObject.book;
            } else if (newItemId == WeaponIDs.WALLET) {
                gameObjectPrefab = weaponScriptableObject.wallet;
            } else if (newItemId == WeaponIDs.IPAD) {
                gameObjectPrefab = weaponScriptableObject.ipad;
            } else {
                gameObjectPrefab = weaponScriptableObject.ipad;
            }

            GameObject itemObject = Instantiate (gameObjectPrefab, itemPos, Quaternion.identity);
            itemObject.transform.parent = transform;
            Destroy(itemObject, 2f);
            hasTakenItem = true;
            box.HideUI ();
        }
    }

    public void OnNearBox(LostAndFoundBox box) {
        nearBox = true;
        this.box = box;
    }

    public void OnLeaveBox() {
        box = null;
        nearBox = false;
        hasTakenItem = false;
    }
}