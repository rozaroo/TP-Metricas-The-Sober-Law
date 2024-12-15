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
            Num = num,
            Level_ID = level_id,
            usuario_identified = user_id
        };

        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
    public void instructionsClicked(int num, string user_id) 
    {
        instructionsClickedEvent evt = new instructionsClickedEvent
        {
            Num = num,
            usuario_identified = user_id
        };
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
    public void medkitPickedUp(int cant, string level_id, string user_id) 
    {
        medkitPickedEvent evt = new medkitPickedEvent
        {
            Cant = cant,
            Level_ID = level_id,
            usuario_identified = user_id
        };
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
    public void playerDeathsPerLevel(int deathsPerLevel, string enemy_id, string level_id, string user_id) 
    {
        playerDeathsPerLevelEvent evt = new playerDeathsPerLevelEvent
        {
            DeathsPerLevel = deathsPerLevel,
            Enemy_ID = enemy_id,
            Level_ID = level_id,
            usuario_identified = user_id
        };
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
    public void gameFinished(float playTime, string user_id) 
    {
        gameFinishedEvent evt = new gameFinishedEvent
        {
            PlayTime = playTime,
            usuario_identified = user_id
        };
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
}
