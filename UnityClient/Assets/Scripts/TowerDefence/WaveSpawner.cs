using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using Random = System.Random;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0; // This one is tricky to fix
    
    [SerializeField] Wave[] waves;
    [SerializeField] Instance[] instances;

    [SerializeField] float timeBetweenWaves = 5f;
    private float _countdown = 2f;

    [SerializeField] Text waveCountdownText;
    [SerializeField] Text enemiesRemaining;
    [SerializeField] Text currentInstance;

    private Transform _currentSpawn;
    public GameObject currentTarget; // Not sure how to fix this

    [SerializeField] GameManager gameManager;

    // Wave Index | Counter
    private int _waveIndex = 0;
    // Instance Index | Counter
    private int _instanceIndex = 0;
    

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
        if (_waveIndex == waves.Length)
        {
            // Next instance
            _instanceIndex++;
            _waveIndex = 0;
            
            if (_instanceIndex == instances.Length)
            {
                gameManager.WinLevel();
            }
        }
        
        // Start coroutine to spawn the waves of enemies (Always at least one wave)
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = timeBetweenWaves;
            return;
        }

        // Reduce countdown by 1 every second
        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", _countdown);
    }

    IEnumerator SpawnWave()
    {

        Wave wave = waves[_waveIndex];

        EnemiesAlive = wave.Count;
        
        Instance instant = instances[_instanceIndex];
        
        _currentSpawn = instant.SpawnPoint;
        currentTarget = instant.Target;
        currentInstance.text = instant.InstanceName;
        
        for (int i = 0; i < wave.Count; i++)
        {
            
            SpawnEnemy(wave.Enemy);
            yield return new WaitForSeconds(1f/ wave.Count);
        }
        _waveIndex++;
    }

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, _currentSpawn.position, _currentSpawn.rotation).GetComponent<Unit>().Init(this);
    }
}
