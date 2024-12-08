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

    private void Awake()
    {
        credits.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleCredits()
    {
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
