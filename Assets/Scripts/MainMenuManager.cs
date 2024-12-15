using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject levelsButtons;
    private int InstructionsPressCount = 0; //Contador de veces que se presiona instrucciones
    public PersistantTimerData PTData;
    private void Awake()
    {
        PTData = GameObject.FindObjectOfType<PersistantTimerData>();
        credits.SetActive(false);
    }
    public void QuitGame()
    {
        PTData.UploadData();
        Application.Quit();
    }
    public void ToggleCredits()
    {
        InstructionsPressCount++;
        // Obtener el user_id desde el GameManager
        string userId = GameManager.Instance.userId;
        //Enviar evento analítico
        AnalyticsManager.instance.instructionsClicked(InstructionsPressCount, userId);
        Debug.Log($"La cantidad de veces que se presiono el boton instrucciones fue {InstructionsPressCount}, por el usuario {userId}");
        credits.SetActive(!credits.active);
    }
    public void LoadLevel1()
    {
        GameManager.Instance.ChangeLevel(1);
    }
    public void LoadLevel2()
    {
        GameManager.Instance.ChangeLevel(2);
    }
    public void LoadLevel3()
    {
        GameManager.Instance.ChangeLevel(3);
    }
    public void ToggleLevels()
    { 
        levelsButtons.SetActive(!levelsButtons.active);
    }
}
