using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTransitions : MonoBehaviour
{
    #region Singleton

    //basic singleton instance
    public static CameraTransitions instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Camera Transitions found!");
            return;
        }

        instance = this;
    }
    //basic singleton instance

    #endregion

    [SerializeField] private GameObject mainCineCam;
    private GameObject _currentActiveCineCam;
    private CinemachineBrain _cinemachineBrain;

    private void Start()
    {
        _cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    public GameObject ReturnCurrentActiveCam()
    {
        return _currentActiveCineCam;
    }

    public void SetNewCam(GameObject newCam, 
        CinemachineBrain.UpdateMethod camUpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate)
    {
        if (_currentActiveCineCam) _currentActiveCineCam.SetActive(false);
        mainCineCam.SetActive(false);
        newCam.SetActive(true);
        _currentActiveCineCam = newCam;
        _cinemachineBrain.m_UpdateMethod = camUpdateMethod;
    }

    public void SetToMainCam()
    {
        if (_currentActiveCineCam != mainCineCam)
        {
            if (_currentActiveCineCam != null)
            {
                _currentActiveCineCam.SetActive(false);
                mainCineCam.SetActive(true);
                _currentActiveCineCam = null;
                _cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
            }
            else
            {
                if(mainCineCam) mainCineCam.SetActive(true);
                _cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
            }
        }
    }
}
