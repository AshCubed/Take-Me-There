using System.Collections;
using System.Collections.Generic;
using VIDE_Data;
using UnityEngine;

public class GodScript : MonoBehaviour
{
    [SerializeField] private Item[] itemsNeededToWin;
    private PlayerInventory playerInventory;
    [SerializeField] private VIDE_Assign VIDE_Assign;
    [SerializeField] private int dialogueHasItems;
    [SerializeField] private int dialogueNotAllItems;
    [SerializeField] private int dialogueNoItems;


    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.instance;
    }

    public void DoesPlayerHaveItems()
    {
        int correctNum = itemsNeededToWin.Length;
        int currentNum = 0;

        foreach (var item in itemsNeededToWin)
        {
            if (playerInventory.SearchInventory(item))
            {
                currentNum++;
            }
        }

        if (VD.isActive && VD.assigned == VIDE_Assign)
        {
            if (currentNum == correctNum)
            {
                VD.SetNode(dialogueHasItems);
                FindObjectOfType<WinDoor>().onWinCallBack.Invoke();
            }
            else if (currentNum == 0)
            {
                VD.SetNode(dialogueNoItems);
            }
            else if (currentNum < correctNum)
            {
                VD.SetNode(dialogueNotAllItems);
            }
        }
    }


}
