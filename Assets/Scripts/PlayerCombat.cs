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
        player = GetComponent<PlayerManager>();
        player.GetActions().Player.Action.performed += OnAction;
    }

    private void OnAction(InputAction.CallbackContext context) {
        if (nearBox && !hasTakenItem && box.hasItems()) {
            int newItemId = box.TakeItem ();
            Vector3 itemPos = transform.position;
            itemPos.y += 1f;

            GameObject gameObjectPrefab;
            if(newItemId == WeaponIDs.UMBRELLA) {
                gameObjectPrefab = weaponScriptableObject.wallet;
            } else if(newItemId == WeaponIDs.TEDDY) {
                gameObjectPrefab = weaponScriptableObject.teddy;
            } else if(newItemId == WeaponIDs.BOOK) {
                gameObjectPrefab = weaponScriptableObject.book;
            } else if(newItemId == WeaponIDs.WALLET) {
                gameObjectPrefab = weaponScriptableObject.wallet;
            } else if(newItemId == WeaponIDs.IPAD) {
                gameObjectPrefab = weaponScriptableObject.ipad;
            } else {
                gameObjectPrefab = weaponScriptableObject.ipad;
            }

            GameObject itemObject = Instantiate (gameObjectPrefab, itemPos, Quaternion.identity);
            itemObject.transform.parent = transform;
            hasTakenItem = true;
            box.SetShowingUI (false);
        }
    }

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.tag == "LostAndFoundBox") {
            box = collision.gameObject.GetComponent<LostAndFoundBox> ();
            box.SetShowingUI (true);
            nearBox = true;
        }
    }

    private void OnCollisionExit (Collision collision) {
        if (collision.gameObject.tag == "LostAndFoundBox") {
            box.SetShowingUI (false);
            box = null;
            nearBox = false;
            hasTakenItem = false;
        }
    }
}