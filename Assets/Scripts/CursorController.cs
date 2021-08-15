using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    #region Singleton

    //basic singleton instance
    public static CursorController instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of CursorController found!");
            return;
        }

        instance = this;
    }
    //basic singleton instance

    #endregion
    
    public enum CursorState {Locked, Unlocked};
    private CursorState cursorState;
    
    // Start is called before the first frame update
    void Start()
    {
        cursorState = CursorController.CursorState.Locked;
        ControlCursor(cursorState);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cursorState == CursorState.Locked)
            {
                cursorState = CursorState.Unlocked;
            }
            else
            {
                cursorState = CursorState.Locked;
            }
        }
        ControlCursor(cursorState);*/
    }

    public void ControlCursor(CursorState state) {
        cursorState = state;
        switch (cursorState)
        {
            case CursorState.Locked:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (MainManager.instance.player)
                {
                    MainManager.instance.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                else
                {
                    Debug.Log("NO PLAYER IN MAIN MANAGER");
                }
                break;
            case CursorState.Unlocked:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (MainManager.instance.player)
                {
                    MainManager.instance.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
                else
                {
                    Debug.Log("NO PLAYER IN MAIN MANAGER");
                }
                break;
            default:
                break;
        }
    }

}//class
