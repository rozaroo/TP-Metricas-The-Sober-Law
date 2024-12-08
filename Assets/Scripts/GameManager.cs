using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    [SerializeField] public GameObject player;
    [SerializeField] Scene currentScene;
    
    private bool isAlive;
    public bool dontDestroyOnLoad;
    public bool isLevel1Completed;
    public bool isLevel2Completed;
    public bool isBossDefeated;
    public static bool isGamePaused;
    public bool gameOver;

    public bool isLevelStarted;

    Vector3 startPos;
    public bool Normal;
    public bool Easy;
    public bool VeryEasy;
    public static event Action<bool> OnGamePauseStateChanged;
    public static void InvokeGamePauseStateChanged(bool isPaused)
    {
        OnGamePauseStateChanged?.Invoke(isPaused);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            else Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        startPos = GameObject.FindGameObjectWithTag("StartPos").GetComponent<Transform>().position;
        currentScene = SceneManager.GetActiveScene();
        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.transform.position = startPos;
        }
    }
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        isLevel1Completed = false;
        isLevel2Completed = false;
        isBossDefeated = false;
        isLevelStarted = false;
        Normal = false;
        Easy = false;
        VeryEasy = false;
        if (!Normal && !Easy && !VeryEasy) ChoseNormal();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) ChoseNormal();
        if (Input.GetKeyDown(KeyCode.Keypad2)) ChoseEasy();
        if (Input.GetKeyDown(KeyCode.Keypad3)) ChoseVeryEasy();
    }
    
    public void LoadMainMenu()
    {
        Unpause();
        SceneManager.LoadScene(0);
    }
    void Unpause()
    {
        Time.timeScale = 1.0f;
        isGamePaused = false;
    }
    /// <summary>
    /// Changes level by level's ID.
    /// </summary>
    /// <param name="scene">Level to be loaded.</param>
    /// <returns>Load a new level scene</returns>
    public void ChangeLevel(int scene)
    {
        Unpause();
        SceneManager.LoadScene(scene);
    }
    /// <summary>
    /// Finds player's prefab on the current scene.
    /// </summary>
    public void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    //Dificultades
    public void ChoseNormal()
    {
        Normal = true;
        Easy = false;
        VeryEasy = false;
    }
    public void ChoseEasy()
    {
        Normal = false;
        Easy = true;
        VeryEasy = false;
    }
    public void ChoseVeryEasy()
    {
        Normal = false;
        Easy = false;
        VeryEasy = true;
    }

}
