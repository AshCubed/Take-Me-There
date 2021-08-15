using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] bool fadeOnStart = false;
    [SerializeField] bool shouldFadeIn;
    [SerializeField] bool shouldDestroy = false;
    [Tooltip("Leave 0 for default value of 1")]
    [SerializeField] float fadePeriod = 0;
    CanvasGroup canvasGroup;
    [SerializeField] Image image;

    // Start is called before the first frame update
    void Start()
    {
        if (fadeOnStart)
        {
            DoTheFade();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoTheFade()
    {
        if (!image.IsActive())
        {
            image.gameObject.SetActive(true);
        }
        canvasGroup = GetComponent<CanvasGroup>();
        if (shouldFadeIn)
        {
            LeanTween.alphaCanvas(canvasGroup, 1, fadePeriod == 0 ? fadePeriod : 1f).setOnComplete(() => {
                if (shouldDestroy)
                {
                    Destroy(gameObject);
                }
            });
        }
        else
        {
            LeanTween.alphaCanvas(canvasGroup, 0, fadePeriod == 0 ? fadePeriod : 1f).setOnComplete(() => {
                if (shouldDestroy)
                {
                    Destroy(gameObject);
                }
            });
        }
    }
}
