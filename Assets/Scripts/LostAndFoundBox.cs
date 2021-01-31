using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LostAndFoundBox : MonoBehaviour
{

  [SerializeField]
  GameObject interactionUI;
  [SerializeField]
  private TextMeshProUGUI uiText;

  private List<int> weaponItemIds;
  [SerializeField]
  private float secondsForRegeneratingItem = 10f;

  void Start()
  {
    weaponItemIds = new List<int>();
    AddRandomItemToBox();
    interactionUI.SetActive(false);

    if(UIManager.Instance.isLevelTutorial()) {
      uiText.text = "Start Game (E)";
    } else {
      AddRandomItemToBox();
      AddRandomItemToBox();
      AddRandomItemToBox();
      AddRandomItemToBox();

      StartCoroutine(RegenerateItemInBox());
    }
  }

  private void AddRandomItemToBox()
  {
    int newItemId = Random.Range(0, 4);
    weaponItemIds.Add(newItemId);
    UIManager.Instance.UpdateItemCountText(weaponItemIds.Count);
  }

  public int TakeItem()
  {
    if (weaponItemIds.Count > 0)
    {
      int itemId = weaponItemIds[0];
      weaponItemIds.RemoveAt(0);
      UIManager.Instance.UpdateItemCountText(weaponItemIds.Count);
      return itemId;
    }
    else
    {
      return -1;
    }
  }

  public bool hasItems()
  {
    return weaponItemIds.Count > 0;
  }

  private IEnumerator RegenerateItemInBox()
  {
    yield return new WaitForSeconds(secondsForRegeneratingItem);

    if (weaponItemIds.Count < 5)
    {
      AddRandomItemToBox();
    }

    StartCoroutine(RegenerateItemInBox());
  }

  public void ShowUI()
  {
    if (hasItems())
    {
      interactionUI.SetActive(true);
    }
  }

  public void HideUI()
  {
    interactionUI.SetActive(false);
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      var player = collision.gameObject.GetComponent<PlayerCombat>();

      if (player)
      {
        player.OnNearBox(this);
      }

      ShowUI();
    }
  }

  private void OnTriggerExit(Collider collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      var player = collision.gameObject.GetComponent<PlayerCombat>();

      if (player)
      {
        player.OnLeaveBox();
      }

      HideUI();
    }
  }
}