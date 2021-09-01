using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VIDE_Data;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject containter_NPC;
    public GameObject container_PLAYER;
    public TextMeshProUGUI text_NPC;
    public TextMeshProUGUI[] text_Choices;
    public KeyCode NextDialogueKey;
    public static DialogueManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        containter_NPC.SetActive(false);
        container_PLAYER.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(NextDialogueKey))
        {
            if (Input.GetKeyDown(NextDialogueKey))
            {
                if (VD.isActive)
                {
                    VD.Next();
                }
            }
        }
    }

    public void Begin(VIDE_Assign dialogueData)
    {
        MainManager.instance.canPlayerMove = false;
        VD.OnNodeChange += UpdateUi;
        VD.OnEnd += End;
        VD.EndDialogue();
        VD.BeginDialogue(dialogueData);
    }

    void UpdateUi(VD.NodeData data)
    {
        containter_NPC.SetActive(false);
        container_PLAYER.SetActive(false);
        if (data.isPlayer)
        {
            container_PLAYER.SetActive(true);

            for (int i = 0; i < text_Choices.Length; i++)
            {
                if (i < data.comments.Length)
                {
                    text_Choices[i].transform.parent.gameObject.SetActive(true);
                    text_Choices[i].text = data.comments[i];
                }
                else
                {
                    text_Choices[i].transform.parent.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (!containter_NPC.activeSelf)
            {
                text_NPC.text = data.comments[data.commentIndex];
                LeanTween.moveLocalY(containter_NPC, 180f, 1f).setOnStart(() =>
                {
                    containter_NPC.SetActive(true);
                });
            }
            else
            {
                text_NPC.text = data.comments[data.commentIndex];
            }
        }
    }


    void End(VD.NodeData data)
    {
        VD.OnNodeChange -= UpdateUi;
        VD.OnEnd -= End;
        VD.EndDialogue();

        LeanTween.moveLocalY(containter_NPC, 713, 1f).setOnComplete(() =>
        {
            containter_NPC.SetActive(false);
            container_PLAYER.SetActive(false);
            text_NPC.text = "";
            CursorController.instance.ControlCursor(CursorController.CursorState.Locked);
            MainManager.instance.canPlayerMove = true;
        });
        
    }

    private void OnDisable()
    {
        if (containter_NPC != null)
        {
            End(null);
        }
    }

    public void SetPlayerChoice(int choice)
    {
        VD.nodeData.commentIndex = choice;
        if (Input.GetMouseButtonUp(0))
        {
            VD.Next();
        }
    }
}
