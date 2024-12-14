using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static EventManager;

public class LevelManager : MonoBehaviour, IPlayerObserver, IHpObserver
{
    //Nivel1
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject defeatScreen;
    [SerializeField] Player player;
    [SerializeField] Image hpFill;
    public InitialScreen initScreen;
    GameObject[] enemiesOnStage;
    public Text enemyCounter;
    public int enemiesToDefeat;
    public int CurrentNivel;
    //Nivel2
    [SerializeField] Text timer;
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    public Scriptableobject Enemies;
    float enemyTimer;
    public float timerChanger;
    float boxTimer;
    public float counter2;

    private bool isLevelCompleted;

    //Nivel3
    public GameObject[] bosses;
    public Transform[] PatrolPoints;
    public GameObject particles;
    ParticleSystem particlesPrefab;
    bool isParticleUsed;
    public int bossDamage;
    public int currentDestination;
    public bool isReturning;
    int currentBossStage;
    [SerializeField] GameObject winnerScreen;
    [SerializeField] Image bossLife;

    //Observer
    private List<IEnemyObserver> enemyObservers = new List<IEnemyObserver>();
    //Eventos
    public static event Action<bool> OnBossDefeatedStateChanged;
    public static event Action Level2LogicEvent;

    private int medikitspicked = 0;
    private int sPressCount = 0; //Contador de veces que se presiona S

    void Start()
    {
        counter2 = 30;
        GameManager.OnGamePauseStateChanged += HandleGamePauseStateChanged;
        OnBossDefeatedStateChanged += HandleBossDefeatedStateChanged;
        if (player != null) 
        { 
            player.RegisterObserver(this);
            player.RegisterHpObserver(this);
        }
        if (CurrentNivel == 1) Level1Funtions();
        if (CurrentNivel == 2) 
        { 
            Level2Funtions();
            Level2LogicEvent += HandleLevel2Logic;
        }
        if (CurrentNivel == 3) Level3Funtions();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.isGamePaused = !GameManager.isGamePaused;
            GameManager.InvokeGamePauseStateChanged(GameManager.isGamePaused);
        }
        if (CurrentNivel == 1)
        {
            //LevelManager
            if (initScreen.canvasGroup.alpha == 1 && GameManager.Instance.isLevel1Completed) GameManager.Instance.ChangeLevel(2);
            if (Input.GetKeyDown(KeyCode.Y)) GameManager.Instance.isLevel1Completed = true;
        }
        if (CurrentNivel == 2) HandleLevel2Logic();
        if (CurrentNivel == 3)
        {
            if (initScreen.canvasGroup.alpha > 0) return;
            BossSetter();
            if (GameManager.Instance.isBossDefeated) 
            {
                //Obtener el ID del nivel actual
                string levelID = SceneManager.GetActiveScene().name;
                // Obtener el user_id desde el GameManager
                string userId = GameManager.Instance.userId;
                AnalyticsManager.instance.ButtonSPressed(sPressCount, levelID, userId);
                Debug.Log($"Tecla S presionada {sPressCount} veces en el nivel {levelID}, por el usuario {userId}");
                OnBossDefeatedStateChanged?.Invoke(true);
            }
            
            UpdateBossLife();
        }
    }
    public void DefeatScreen()
    {
        if (defeatScreen == null) return;
        defeatScreen.SetActive(true);
    }
    public void MainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
    public void Exit()
    {
        Application.Quit();
    }

    void RefreshHPBar()
    {
        hpFill.fillAmount = player.PlayerHealth / 100f;
    }
    void GetEnemiesAmount()
    {
        enemiesToDefeat = 0;
        for (int i = 0; i <= enemiesOnStage.Length - 1; i++)
            enemiesToDefeat++;
    }
    
    internal void StartCoroutine()
    {
        throw new NotImplementedException();
    }
    void PauseMenu()
    {
        if (gameplayUI == null) return;
        gameplayUI.SetActive(!gameplayUI.activeSelf);
        pauseUI.SetActive(!pauseUI.activeSelf);
    }
    private void Level1Funtions()
    {
        gameplayUI.SetActive(true);
        pauseUI.SetActive(false);
        defeatScreen.SetActive(false);
        //LevelManager
        GameManager.Instance.isLevel1Completed = false;
        enemiesOnStage = GameObject.FindGameObjectsWithTag("Enemy");
        GetEnemiesAmount();
        enemyCounter = GameObject.FindGameObjectWithTag("Counter").GetComponent<Text>();
        enemyCounter.text = enemiesToDefeat.ToString();
        NotifyEnemyDead();
    }
    private void Level2Funtions()
    {
        timer.text = "30";
        gameplayUI.SetActive(true);
        pauseUI.SetActive(false);
        defeatScreen.SetActive(false);
        //LevelManager
        initScreen.isLevelEnded = false;
        counter2 = 30;
        timerChanger = 3;
        enemyTimer = 4;
        NotifyEnemyDead();
    }
    
    private void Level3Funtions()
    {
        bossDamage = 0;
        isReturning = false;
        currentDestination = 3;
        GameManager.Instance.isBossDefeated = false;
        particlesPrefab = particles.GetComponent<ParticleSystem>();
        for (int i = 0; i < bosses.Length; i++)
            bosses[i].SetActive(false);
        //UI
        gameplayUI.SetActive(true);
        pauseUI.SetActive(false);
        defeatScreen.SetActive(false);
        winnerScreen.SetActive(false);
        //UpdateBossLife()
        bossLife.fillAmount = 1f;
    }
    //Nivel3JefeFunciones
    void BossSetter()
    {
        if (bossDamage >= 100)
        {
            bosses[2].SetActive(false);
            GameManager.Instance.isBossDefeated |= true;
            return;
        }
        if (bossDamage >= 75)
        {
            StartCoroutine(BossChanger(bosses[1], bosses[2]));
            return;
        }
        if (bossDamage >= 50)
        {
            StartCoroutine(BossChanger(bosses[0], bosses[1]));
            return;
        }
        if (bossDamage < 50) bosses[0].SetActive(true);
    }

    void ChangeBoss(GameObject defeatedBoss, GameObject newBoss)
    {
        if (!newBoss.active)
        {
            defeatedBoss.SetActive(false);
            newBoss.SetActive(true);
        }
    }

    IEnumerator BossChanger(GameObject bossDefeated, GameObject newBoss)
    {
        if (!newBoss.active)
        {
            bossDefeated.SetActive(false);
            yield return new WaitForSeconds(2);
            newBoss.SetActive(true);
        }
    }

    public void LifeUpdate(int damage)
    {
        bossDamage += damage * 2;
    }
    public void OnPlayerDead()
    {
        GameManager.Instance.player.SetActive(false);
        DefeatScreen();
    }
    public void OnHpChanged()
    {
        if (CurrentNivel == 1) RefreshHPBar();
        if (CurrentNivel == 2) RefreshHPBar();
        if (CurrentNivel == 3) RefreshHPBar();
    }
    //Observers
    public void RegisterObserver(IEnemyObserver observer)
    {
        enemyObservers.Add(observer);
    }
    public void UnregisterObserver(IEnemyObserver observer)
    {
        enemyObservers.Remove(observer);
    }
    private void NotifyEnemyDead()
    {
        foreach (var observer in enemyObservers)
            observer.OnKilledEnemy();
    }
    public void OnKilledEnemy()
    {
        if (CurrentNivel == 1)
        {
            enemiesToDefeat--;
            if (enemyCounter == null) enemyCounter = GameObject.FindGameObjectWithTag("Counter").GetComponent<Text>();
            enemyCounter.text = enemiesToDefeat.ToString();
            if (enemiesToDefeat == 0) 
            {
                GameManager.Instance.isLevel1Completed = true;
                //Enviar evento analítico
                //Obtener el ID del nivel actual
                string levelID = SceneManager.GetActiveScene().name;
                // Obtener el user_id desde el GameManager
                string userId = GameManager.Instance.userId;
                AnalyticsManager.instance.medkitPickedUp(medikitspicked, levelID, userId);
                Debug.Log($"La cantidad de botiquines recojidos fue {medikitspicked}, en el nivel {levelID}, por el usuario {userId}");
                AnalyticsManager.instance.ButtonSPressed(sPressCount, levelID, userId);
                Debug.Log($"Tecla S presionada {sPressCount} veces en el nivel {levelID}, por el usuario {userId}");
            }


        }
        if (CurrentNivel == 2)
        {
            if (timerChanger > 0.8f) timerChanger -= 0.1f;
        }
    }
    //Menú de Pausa
    public void Pausa()
    {
        GameManager.isGamePaused = true;
        Time.timeScale = 0f;
    }
    public void Reanudar()
    {
        GameManager.isGamePaused = false;
        Time.timeScale = 1f;
        gameplayUI.SetActive(true);
        pauseUI.SetActive(false);
        
    }
    public void Reiniciar()
    {
        GameManager.isGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //Evento
    void HandleGamePauseStateChanged(bool isPaused)
    {
        if (CurrentNivel == 1 || CurrentNivel == 3)
        {
            if (isPaused)
            {
                Pausa();
                gameplayUI.SetActive(false);
                pauseUI.SetActive(true);
            }
            else
            {
                Reanudar();
                gameplayUI.SetActive(true);
                pauseUI.SetActive(false);
            }
        }
        if (CurrentNivel == 2)
        {
            if (!isPaused)
            {
                Reanudar();
                gameplayUI.SetActive(true);
                pauseUI.SetActive(false);
            }
            else
            {
                Pausa();
                gameplayUI.SetActive(false);
                pauseUI.SetActive(true);
            }
        }
    }
    void OnDestroy()
    {
        GameManager.OnGamePauseStateChanged -= HandleGamePauseStateChanged;
    }
    void HandleBossDefeatedStateChanged(bool isDefeated)
    {
        if (isDefeated && winnerScreen != null) winnerScreen.SetActive(true);
    }
    void UpdateBossLife()
    {
        bossLife.fillAmount = 1 - bossDamage / 100f;
    }
    void HandleLevel2Logic()
    {
        if (!GameManager.isGamePaused) timer.text = Mathf.RoundToInt(counter2).ToString();

        if (counter2 <= 0)
        {
            initScreen.isLevelEnded = true;
            //Enviar evento analítico
            //Obtener el ID del nivel actual
            string levelID = SceneManager.GetActiveScene().name;
            // Obtener el user_id desde el GameManager
            string userId = GameManager.Instance.userId;
            AnalyticsManager.instance.medkitPickedUp(medikitspicked, levelID, userId);
            Debug.Log($"La cantidad de botiquines recojidos fue {medikitspicked}, en el nivel {levelID}, por el usuario {userId}");
            AnalyticsManager.instance.ButtonSPressed(sPressCount, levelID, userId);
            Debug.Log($"Tecla S presionada {sPressCount} veces en el nivel {levelID}, por el usuario {userId}");
            if (initScreen.GetComponent<CanvasGroup>().alpha == 1) GameManager.Instance.ChangeLevel(3);
        }

        if (!GameManager.isGamePaused && initScreen.canvasGroup.alpha <= 0)
        {
            enemyTimer += Time.deltaTime;
            boxTimer += Time.deltaTime;
            if (counter2 > 0) counter2 -= Time.deltaTime;
            if (enemyTimer >= timerChanger)
            {
                Instantiate(Enemies.Enemies[Mathf.RoundToInt(UnityEngine.Random.Range(0, 3))],
                            spawnPoints[Mathf.RoundToInt(UnityEngine.Random.Range(0, 6))].position,
                            transform.rotation);
                enemyTimer = 0;
            }

            if (boxTimer >= 2)
            {
                Instantiate(Enemies.Enemies[3],
                            spawnPoints[Mathf.RoundToInt(UnityEngine.Random.Range(0, 6))].position,
                            transform.rotation);
                boxTimer = 0;
            }
        }
    }
    public void IncrementMedikitCount()
    {
        medikitspicked++;
    }
    public void IncrementScounter() 
    {
        sPressCount++;
    }
}
