using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject hinge;
    private bool isDoorOpen;

    // Start is called before the first frame update
    void Start()
    {
        isDoorOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MainManager.instance.GetPlayerTag()) && !isDoorOpen)
        {
            isDoorOpen = true;
            Vector3 playerDir = hinge.transform.InverseTransformPoint(other.transform.position);
            LeanTween.moveLocalY(hinge, 20f, 1f);
            AudioManager.instance.PlaySounds("DoorOpen");
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(MainManager.instance.GetPlayerTag()))
        {
            if (isDoorOpen)
            {
                isDoorOpen = false;
                //LeanTween.rotateY(hinge, 0f, 1f);
                LeanTween.moveLocalY(hinge, 0.6515778f, 1f);
                AudioManager.instance.PlaySounds("DoorOpen");
            }
        }
    }

}
