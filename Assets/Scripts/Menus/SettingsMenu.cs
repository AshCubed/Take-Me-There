using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu instance;

    [Space(4)]
    public GameObject settingsPanel;
    public AudioMixer audioMixer;
    public Slider sldrMainV;
    public Slider sldrSfxV;
    public Slider sldrUiV;
    public Slider sldrMusicV;
    public Toggle tglFullscreen;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualtyDropdown;
    private Resolution[] _resolutions;
    private bool canPause;
    private float timeScale;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timeScale = Time.timeScale;
        canPause = true;
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();  
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.width)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value);});
        qualtyDropdown.onValueChanged.AddListener(delegate { SetQuality(qualtyDropdown.value); });


        GamePause(false);
        sldrMainV.onValueChanged.AddListener(delegate { SetMainVolume(sldrMainV.value); });
        sldrSfxV.onValueChanged.AddListener(delegate { SetSfxMainVolume(sldrSfxV.value); });
        sldrMusicV.onValueChanged.AddListener(delegate { SetMusicVolume(sldrMusicV.value); });
        sldrUiV.onValueChanged.AddListener(delegate { SetUiMainVolume(sldrUiV.value); });
        tglFullscreen.onValueChanged.AddListener(delegate { SetFullScreen(tglFullscreen.isOn); });

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.CompareTo("MainMenu") != 0)
        {
            if (Input.GetKeyUp(KeyCode.Escape) && canPause)
            {
                canPause = false;
                GamePause(true);
                LeanTween.moveLocalY(settingsPanel, 0, 1f)
                    .setOnStart(() => { settingsPanel.SetActive(true); })
                    .setOnComplete(() =>
                    {
                        canPause = true;
                    //Time.timeScale = 0.0001f;
                    CursorController.instance.ControlCursor(CursorController.CursorState.Unlocked);
                    });
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                //Time.timeScale = timeScale;
                LeanTween.moveLocalY(settingsPanel, 1222, 1f)
                    .setOnComplete(() =>
                    {
                        canPause = true;
                        settingsPanel.SetActive(false);
                        GamePause(false);
                        CursorController.instance.ControlCursor(CursorController.CursorState.Locked);
                    });
            }
        }
    }
    
    void GamePause(bool x)
    {
        if (SceneManager.GetActiveScene().name.CompareTo("MainMenu") != 0)
        {
            MainManager.instance.isGamePaused = x;
            MainManager.instance.canPlayerMove = !x;
        }
            
    }

    #region VolumeControls
    public void SetMainVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSfxMainVolume(float volume)
    {
        
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetUiMainVolume(float volume)
    {
        audioMixer.SetFloat("UiVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    #endregion

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(QualitySettings.GetQualityLevel());
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainLevel");
        LeanTween.moveLocalY(settingsPanel, 1222, 1f)
                    .setOnComplete(() =>
                    {
                        canPause = true;
                        settingsPanel.SetActive(false);
                        GamePause(false);
                        CursorController.instance.ControlCursor(CursorController.CursorState.Locked);
                    });
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ApplicationExit()
    {
        Application.Quit();
    }
}
