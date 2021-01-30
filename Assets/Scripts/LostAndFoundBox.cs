using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostAndFoundBox : MonoBehaviour
{

  [SerializeField]
  GameObject interactionUI;

  private List<int> weaponItemIds;
  [SerializeField]
  private float secondsForRegeneratingItem = 10f;

  void Start()
  {
    interactionUI.SetActive(false);
    weaponItemIds = new List<int>();
    AddRandomItemToBox();
    AddRandomItemToBox();
    AddRandomItemToBox();
    AddRandomItemToBox();
    AddRandomItemToBox();

    StartCoroutine(RegenerateItemInBox());
  }

  private void AddRandomItemToBox()
  {
    int newItemId = Random.Range(0, 4);
    weaponItemIds.Add(newItemId);
  }

  public int TakeItem()
  {
    if (weaponItemIds.Count > 0)
    {
      int itemId = weaponItemIds[0];
      weaponItemIds.RemoveAt(0);
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
      print("New item added to box!");
    }
    else
    {
      print("Box is already full.");
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
}