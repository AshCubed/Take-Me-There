using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinSceneScript : MonoBehaviour
{
    public string mainLevel;
    public FadeInOut fadeInOut;
    // Start is called before the first frame update
    void Start()
    {
        fadeInOut = FindObjectOfType<FadeInOut>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        fadeInOut.DoTheFade();
        StartCoroutine(Wait(mainLevel));
    }

    public void LoadMainMenu()
    {
        fadeInOut.DoTheFade();
        StartCoroutine(Wait("MainMenu"));
    }

    IEnumerator Wait(string name)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }

    public void ApplicationExit()
    {
        Application.Quit();
    }
}
