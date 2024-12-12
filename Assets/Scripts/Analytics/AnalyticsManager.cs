using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    // Roberto
    public static AnalyticsManager instance;
    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            GiveConsent();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }


    public void GiveConsent()
    {
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log($"Consent given! We can get the data!!!");
    }

    public void ButtonSPressed(int num, string level_id, string user_id)
    {
        ButtonSPressedEvent evt = new ButtonSPressedEvent
        {
            number = num,
            level_id = level_id,
            user_id = user_id
        };

        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
}
