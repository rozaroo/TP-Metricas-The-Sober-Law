using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    float counter;
    bool isCounterCompleted;
    public bool isLevelEnded;
    Text[] titles;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        titles = GetComponentsInChildren<Text>();
    }

    void Start()
    {
        isCounterCompleted = false;
        isLevelEnded = false;
        counter = 0;
        canvasGroup.alpha = 1;
    }

    void Update()
    {
        if (!isCounterCompleted)
        {
            counter += Time.deltaTime;
            if (counter >= 3) canvasGroup.alpha -= Time.deltaTime;
            if (counter >= 5 && canvasGroup.alpha == 0)
            {
                isCounterCompleted = true;
                GameManager.Instance.isLevelStarted = true;
            }
        }
        if (isLevelEnded)
        {
            canvasGroup.alpha += Time.deltaTime;
            titles[0].text = "";
            titles[1].text = "";
        }
    }
}
