using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using Random = System.Random;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float initialCountdown = 2f;

    public Text waveCountdownText;

    private int waveIndex = 1;

    private void Update()
    {
        if (initialCountdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            initialCountdown = timeBetweenWaves;
        }

        // Reduce countdown by 1 every second
        initialCountdown -= Time.deltaTime;

        waveCountdownText.text = Math.Round(initialCountdown).ToString();

        if (waveIndex == 4)
        {
            this.enabled = false;
        }
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
         int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
         
         Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
