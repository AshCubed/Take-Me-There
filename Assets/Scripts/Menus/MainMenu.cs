using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settinsPanel;
    public GameObject mainMenuPanel;
    public string mainLevelToLoad;
    private FadeInOut fadeInOut;

    // Start is called before the first frame update
    void Start()
    {
        fadeInOut = FindObjectOfType<FadeInOut>();
        //AudioManager.instance.PlaySounds("Atmosphere1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainLevel()
    {
        fadeInOut.DoTheFade();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(mainLevelToLoad);
    }

    public void OpenSettingsMenu()
    {
        LeanTween.moveLocalY(mainMenuPanel, 1090, 1f)
            .setOnStart(() =>
            {
                LeanTween.moveLocalY(settinsPanel, 0, 1f)
                .setOnStart(() => 
                {
                    settinsPanel.SetActive(true);
                });
            });
    }

    public void CloseSettingsMenu()
    {
        LeanTween.moveLocalY(mainMenuPanel, 0, 1f)
            .setOnStart(() =>
            {
                LeanTween.moveLocalY(settinsPanel, -1090, 1f)
                .setOnComplete(() =>
                {
                    settinsPanel.SetActive(false);
                });
            });
    }

    public void ApplicationExit()
    {
        Application.Quit();
    }
}
