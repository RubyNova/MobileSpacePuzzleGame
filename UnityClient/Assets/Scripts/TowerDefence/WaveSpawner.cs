using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using Random = System.Random;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;
    
    public Wave[] waves;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public Text waveCountdownText;
    public Text enemiesRemaining;

    public GameManager gameManager;

    private int waveIndex = 0;

    private void Update()
    {
        
        enemiesRemaining.text = EnemiesAlive.ToString();
        
        if (EnemiesAlive > 0)
        {
            return;
        }
        
        if (waveIndex == waves.Length)
        {
            gameManager.WinLevel();
            this.enabled = false;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        // Reduce countdown by 1 every second
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
        
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;

        
        
        for (int i = 0; i < wave.count; i++)
        {
            
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f/ wave.rate);
        }
        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
