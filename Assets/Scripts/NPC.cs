using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(VIDE_Assign))]
public class NPC : MonoBehaviour
{
    [SerializeField] private VIDE_Assign VIDE_Assign;
    private TextMeshProUGUI tmpNPCTalk;
    private Color myColor;
    // Start is called before the first frame update
    void Start()
    {
        tmpNPCTalk = GameObject.FindGameObjectWithTag("talkNPC").GetComponent<TextMeshProUGUI>();
        myColor = tmpNPCTalk.color;
        myColor.a = 0;
        tmpNPCTalk.color = myColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MainManager.instance.GetPlayerTag()))
        {
            myColor.a = 1;
            tmpNPCTalk.color = myColor;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == MainManager.instance.GetPlayerTag()) && Input.GetKey(KeyCode.E))
        {
            if (Cursor.lockState == CursorLockMode.Locked)//if cursor locked unlock it
            {
                CursorController.instance.ControlCursor(CursorController.CursorState.Unlocked);
            }
            DialogueManager.Instance.Begin(VIDE_Assign);
            MainManager.instance.canPlayerMove = false;
            myColor.a = 0;
            tmpNPCTalk.color = myColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(MainManager.instance.GetPlayerTag()))
        {
            myColor.a = 0;
            tmpNPCTalk.color = myColor;
        }
    }
}
