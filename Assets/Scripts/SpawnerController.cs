using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    GameObject[] enemies;

    float enemyTimer;
    float timerChanger;
    float boxTimer;

    void Start()
    {
        timerChanger = 3;
        enemyTimer = 4;
    }

    void Update()
    {
        enemyTimer += Time.deltaTime;
        boxTimer += Time.deltaTime;

        if (enemyTimer >= timerChanger)
        {
            Instantiate(enemies[Mathf.RoundToInt(Random.Range(0, 3))],
                        spawnPoints[Mathf.RoundToInt(Random.Range(0, 6))].position,
                        transform.rotation);
            enemyTimer = 0;
            timerChanger -= 0.1f;
        }

        if (boxTimer >= 2)
        {
            Instantiate(enemies[3],
                        spawnPoints[Mathf.RoundToInt(Random.Range(0, 6))].position,
                        transform.rotation);
            boxTimer = 0;
        }
    }
}
