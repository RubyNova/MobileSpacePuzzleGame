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
    
    [SerializeField] Wave[] waves;
    [SerializeField] Instance[] instances;

    [SerializeField] float timeBetweenWaves = 5f;
    private float countdown = 2f;

    [SerializeField] Text waveCountdownText;
    [SerializeField] Text enemiesRemaining;
    [SerializeField] Text currentInstance;

    private Transform currentSpawn;
    public GameObject currentTarget;

    [SerializeField] GameManager gameManager;

    // Wave Index | Counter
    private int waveIndex = 0;
    // Instance Index | Counter
    private int instanceIndex = 0;
    

    private void Update()
    {

        //Display the enemies left in a text element
        enemiesRemaining.text = "Enemies Left : " + EnemiesAlive;

        // If the enemies are all dead then spawn the next wave 
        if (EnemiesAlive > 0)
        {
            return;
        }
        
        // If the last wave has been reached and the last level instance has been played through
        if (waveIndex == waves.Length)
        {
            // Next instance
            instanceIndex++;
            waveIndex = 0;
            
            if (instanceIndex == instances.Length)
            {
                gameManager.WinLevel();
            }
        }
        
        // Start coroutine to spawn the waves of enemies (Always at least one wave)
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

        EnemiesAlive = wave.Count;
        
        Instance instant = instances[instanceIndex];
        
        currentSpawn = instant.SpawnPoint;
        currentTarget = instant.Target;
        currentInstance.text = instant.InstanceName;
        
        for (int i = 0; i < wave.Count; i++)
        {
            
            SpawnEnemy(wave.Enemy);
            yield return new WaitForSeconds(1f/ wave.Count);
        }
        waveIndex++;
    }

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, currentSpawn.position, currentSpawn.rotation).GetComponent<Unit>().Init(this);
    }
}
