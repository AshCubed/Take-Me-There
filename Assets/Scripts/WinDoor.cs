using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    public GameObject hinge;
    public string winSceneName;
    private bool canDoorOpen;

    public delegate void OnWin();
    public OnWin onWinCallBack;

    // Start is called before the first frame update
    void Start()
    {
        canDoorOpen = false;
        onWinCallBack += SetWin;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            SetWin();
        }*/
    }

    void SetWin()
    {
        canDoorOpen = true;
        Vector3 playerDir = hinge.transform.InverseTransformPoint(MainManager.instance.player.transform.position);
        LeanTween.moveLocalY(hinge, 20f, 1f);
        //return 1 when x is + or 0, -1 when x is negative
        /*if (Mathf.Sign(playerDir.x) == 1)
        {
            LeanTween.rotateY(hinge, 120f, 1f);
        }
        else
        {
            LeanTween.rotateY(hinge, -120f, 1f);
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDoorOpen)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        FindObjectOfType<FadeInOut>().DoTheFade();
        AudioManager.instance.PlaySounds("Footsteps");
        MainManager.instance.canPlayerMove = false;
        yield return new WaitForSeconds(AudioManager.instance.GetSound("Footsteps").clip.length);
        CursorController.instance.ControlCursor(CursorController.CursorState.Unlocked);
        SceneManager.LoadScene(winSceneName);
    }
}
