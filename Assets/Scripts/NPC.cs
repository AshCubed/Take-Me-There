using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(VIDE_Assign))]
public class NPC : MonoBehaviour
{
    [SerializeField] private VIDE_Assign VIDE_Assign;
    private TextMeshProUGUI tmpNPCTalk;
    private MeshRenderer npcMeshRenderer;
    private Camera cam;
    private Color myColor;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        npcMeshRenderer = GetComponent<MeshRenderer>();
        tmpNPCTalk = GameObject.FindGameObjectWithTag("talkNPC").GetComponent<TextMeshProUGUI>();
        myColor = tmpNPCTalk.color;
        myColor.a = 0;
        tmpNPCTalk.color = myColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MainManager.instance.GetPlayerTag()) && npcMeshRenderer.IsVisibleFrom(cam))
        {
            myColor.a = 1;
            tmpNPCTalk.color = myColor;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(MainManager.instance.GetPlayerTag()) && npcMeshRenderer.IsVisibleFrom(cam))
        {
            if (MainManager.instance.canPlayerMove)
            {
                myColor.a = 1;
                tmpNPCTalk.color = myColor;
            }
            else
            {
                myColor.a = 0;
                tmpNPCTalk.color = myColor;
            }
        }

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
