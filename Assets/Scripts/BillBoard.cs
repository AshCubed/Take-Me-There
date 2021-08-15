using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] private bool useStaticBillboard;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        BillBoarding();
    }

    void BillBoarding()
    {
        if (!useStaticBillboard)
        {
            transform.LookAt(cam.transform);
        }
        else
        {
            transform.rotation = cam.transform.rotation;
        }

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
