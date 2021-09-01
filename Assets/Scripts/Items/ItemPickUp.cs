using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    GameObject player;
    PlayerInventory playerInventory;

    private void Start()
    {
        player = MainManager.instance.player;
        playerInventory = PlayerInventory.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            AudioManager.instance.PlaySounds("ItemCollect");
            playerInventory.PickUpItem(item);
            Destroy(gameObject);
        }
    }
}
