using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    GameObject player;
    bool canPickUp;
    PlayerInventory playerInventory;
    private Renderer itemRenderer;
    private Camera cam;


    private void Start()
    {
        cam = Camera.main;
        canPickUp = false;
        player = MainManager.instance.player;
        playerInventory = PlayerInventory.instance;
        itemRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (canPickUp)
        {
            if (itemRenderer.IsVisibleFrom(cam))
            {
                playerInventory.SetText(item, true);
            }
            else
            {
                playerInventory.SetText(null, false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            canPickUp = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (canPickUp)
        {
            if (Input.GetKeyUp(KeyCode.E) && itemRenderer.IsVisibleFrom(cam))
            {
                AudioManager.instance.PlaySounds("ItemCollect");
                playerInventory.PickUpItem(item);
                playerInventory.SetText(null, false);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            canPickUp = false;
        }
    }
}
