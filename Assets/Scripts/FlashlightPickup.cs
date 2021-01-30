using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
            player.SetFlashlightOn(true);
            Destroy(gameObject);
        }
    }
}
