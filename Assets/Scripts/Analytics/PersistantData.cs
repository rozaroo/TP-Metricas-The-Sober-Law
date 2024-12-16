using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantData : MonoBehaviour
{
    public static PersistantData Instance { get; private set; }
    private int deathsinlevelone;
    private int deathsinleveltwo;
    private int deathsinlevelthree;
    private bool dontDestroyOnLoad = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            else Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

    public void IncrementDeaths()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                deathsinlevelone++;
                Debug.Log($"Muertes registradas en Nivel 1: {deathsinlevelone}");
                UploadData();
                break;
            case 2:
                deathsinleveltwo++;
                break;
            case 3:
                deathsinlevelthree++;
                break;
            default:
                break;
        }
    }
    public void UploadData()
    {
        if (GameManager.Instance == null || GameManager.Instance.userId == null)
        {
            Debug.LogError("GameManager o userId es null al intentar subir datos.");
            return;
        }
        string CommonEnemy = "Mafioso común";
        string BossEnemy = "Jefe de la mafia";
        //Obtener el ID del nivel actual
        string levelID = SceneManager.GetActiveScene().name;
        // Obtener el user_id desde el GameManager
        string userId = GameManager.Instance.userId;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            AnalyticsManager.instance.playerDeathsPerLevel(deathsinlevelone, CommonEnemy, levelID, userId);
            Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {levelID} fue {deathsinlevelone} por un {CommonEnemy}");
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            AnalyticsManager.instance.playerDeathsPerLevel(deathsinleveltwo, CommonEnemy, levelID, userId);
            Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {levelID} fue {deathsinleveltwo} por un {CommonEnemy}");
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            AnalyticsManager.instance.playerDeathsPerLevel(deathsinlevelthree, BossEnemy, levelID, userId);
            Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {levelID} fue {deathsinlevelthree} por el {BossEnemy}");
        }
    }
}
