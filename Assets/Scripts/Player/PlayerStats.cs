using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    [Header("Sanity Bar")]
    [SerializeField] private Image loadingImage;

    [Header("Taking Damage")]
    [SerializeField] private Volume psychadelicVolume;

    [Header("Death Stuff")]
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private CanvasGroup imagesGroup;
    [SerializeField] private CanvasGroup btnsGroup;
    [SerializeField] private GameObject image0;
    [SerializeField] private GameObject image1;

    [Header("Checkpoint")]
    [SerializeField] private GameObject txtCheckPoint;


    public delegate void OnNewCheckpoint(Transform newPoint);
    public OnNewCheckpoint onNewCheckpointCallback;
    private Vector3 checkpointPos;

    public override void Start()
    {
        base.Start();
        txtCheckPoint.SetActive(false);
        Color c = txtCheckPoint.GetComponent<TextMeshProUGUI>().color; ;
        c.a = 0;
        txtCheckPoint.GetComponent<TextMeshProUGUI>().color = c;
        psychadelicVolume.weight = 0;
        loadingImage.fillAmount = 1;
        onNewCheckpointCallback += NewCheckPoint;
        checkpointPos = transform.position;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Color myColor = loadingImage.color;
        if (health.GetCurrentValue() <= 20)
        {
            myColor = new Color32(0, 0, 24, 255);
        }
        else if (health.GetCurrentValue() <= 40)
        {
            myColor = new Color32(39, 11, 40, 255);
        }
        else if (health.GetCurrentValue() <= 60)
        {
            myColor = new Color32(73, 21, 55, 255);
        }
        else if (health.GetCurrentValue() <= 80)
        {
            myColor = new Color32(89, 25, 61, 255);
        }
        else if (health.GetCurrentValue() <= 100)
        {
            myColor = new Color32(100, 29, 66, 255);
        }

        loadingImage.color = myColor;

        float num = 1.0f - health.GetCurrentValue() % health.GetMaxValue() * 0.01f;
        LeanTween.value(psychadelicVolume.gameObject, psychadelicVolume.weight, num, .5f)
            .setOnUpdate((float val) => { psychadelicVolume.weight = val;});
        float num1 = health.GetCurrentValue() % health.GetMaxValue() * 0.01f;
        LeanTween.value(loadingImage.gameObject, loadingImage.fillAmount, num1, .5f)
            .setOnUpdate((float val) => { loadingImage.fillAmount = val; });
    }

    

    public override void Die()
    {
        base.Die();
        AudioManager.instance.PlaySounds("Death");
        MainManager.instance.canPlayerMove = false;
        deathCanvas.SetActive(true);
        LeanTween.alphaCanvas(imagesGroup, 1, .3f).setOnComplete(() => {
            LeanTween.alphaCanvas(btnsGroup, 1, .3f);
        });

        image0.transform.LeanMoveLocalY(400, .7f);

        image1.transform.LeanMoveLocalY(-450, .7f);

        CursorController.instance.ControlCursor(CursorController.CursorState.Unlocked);
    }

    void NewCheckPoint(Transform newPoint)
    {
        checkpointPos = newPoint.position;
        txtCheckPoint.SetActive(true);
        Color c = txtCheckPoint.GetComponent<TextMeshProUGUI>().color;

        LeanTween.value(txtCheckPoint.gameObject, c.a, 1, .5f)
            .setOnUpdate((float val) => {
                c.a = val;
                txtCheckPoint.GetComponent<TextMeshProUGUI>().color = c;
            }).setOnComplete(() => {
                LeanTween.value(txtCheckPoint.gameObject, c.a, 0, .7f)
                .setOnUpdate((float val) =>{
                    c.a = val;
                    txtCheckPoint.GetComponent<TextMeshProUGUI>().color = c;
                }).setOnComplete(() => txtCheckPoint.SetActive(false));
            });
    }

    public void Continue()
    {
        transform.position = checkpointPos;
        health.ResetStat();
        psychadelicVolume.weight = 0;

        Color myColor = new Color32(255, 74, 169, 255);
        loadingImage.color = myColor;

        float num1 = health.GetCurrentValue() % health.GetMaxValue() * 0.01f;
        LeanTween.value(loadingImage.gameObject, loadingImage.fillAmount, 1f, .5f)
            .setOnUpdate((float val) => { loadingImage.fillAmount = val; });
        isDead = false;

        LeanTween.alphaCanvas(btnsGroup, 0, 1f)
            .setOnComplete(() =>
            {
                LeanTween.alphaCanvas(imagesGroup, 0, .7f);
                image0.transform.LeanMoveLocalY(904, 1f);
                image1.transform.LeanMoveLocalY(-1131, 1f);
                StartCoroutine(Timer());
            });
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(.7f);
        Debug.Log("HERE");
        CursorController.instance.ControlCursor(CursorController.CursorState.Locked);
        MainManager.instance.canPlayerMove = true;
        deathCanvas.SetActive(false);
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
