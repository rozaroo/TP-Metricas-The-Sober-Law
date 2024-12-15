using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantData : MonoBehaviour
{
    public LevelManager lvlmanager;
    public static PersistantData Instance { get; private set; }
    private int deathsinlevelone = 0;
    private int deathsinleveltwo = 0;
    private int deathsinlevelthree = 0;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        lvlmanager= GameObject.FindObjectOfType<LevelManager>();
        if (lvlmanager == null) Debug.LogError("No se encontró el LevelManager en la escena.");
    }
    public void IncrementDeaths()
    {
        if (lvlmanager.CurrentNivel == 1) deathsinlevelone++;
        if (lvlmanager.CurrentNivel == 2) deathsinleveltwo++;
        if (lvlmanager.CurrentNivel == 3) deathsinlevelthree++;
    }
    public void UploadData()
    {
        string CommonEnemy = "Mafioso común";
        string BossEnemy = "Jefe de la mafia";
        //Obtener el ID del nivel actual
        string levelID = SceneManager.GetActiveScene().name;
        // Obtener el user_id desde el GameManager
        string userId = GameManager.Instance.userId;
        if (lvlmanager.CurrentNivel == 1)
        {
            AnalyticsManager.instance.playerDeathsPerLevel(deathsinlevelone, CommonEnemy, levelID, userId);
            Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {levelID} fue {deathsinlevelone} por un {CommonEnemy}");
        }
        if (lvlmanager.CurrentNivel == 2)
        {
            AnalyticsManager.instance.playerDeathsPerLevel(deathsinleveltwo, CommonEnemy, levelID, userId);
            Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {levelID} fue {deathsinleveltwo} por un {CommonEnemy}");
        }
        if (lvlmanager.CurrentNivel == 3)
        {
            AnalyticsManager.instance.playerDeathsPerLevel(deathsinleveltwo, CommonEnemy, levelID, userId);
            Debug.Log($"La cantidad de veces que murio el jugador {userId} en el nivel {levelID} fue {deathsinlevelthree} por el {BossEnemy}");
        }
    }
}
