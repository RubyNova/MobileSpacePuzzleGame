using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class WaveSpawner : MonoBehaviour
{
    public int EnemiesAlive { get; set; }
    public GameObject CurrentTarget { get; private set; }
    
    private float _countdown = 2f;
    
    private Transform _currentSpawn;
    // Index Counters
    private int _waveIndex = 0;
    private int _instanceIndex = 0;
    
    [Header("Wave Attributes")]
    [FormerlySerializedAs("timeBetweenWaves")] [SerializeField] private float _timeBetweenWaves = 5f;
    [FormerlySerializedAs("waves")] [SerializeField] private Wave[] _waves;
    [Header("Instance Attributes")]
    [FormerlySerializedAs("instances")] [SerializeField] private Instance[] _instances;
    
    [Header("UI Visualisation")]
    [FormerlySerializedAs("waveCountdownText")] [SerializeField] private Text _waveCountdownText;
    [FormerlySerializedAs("enemiesRemaining")] [SerializeField] private Text _enemiesRemaining;
    [FormerlySerializedAs("currentInstance")] [SerializeField] private Text _currentInstance;

    [Header("GameManager Script")]
    [FormerlySerializedAs("gameManager")] [SerializeField] private GameManager _gameManager;

    private void Update()
    {
        //Display the enemies left in a text element
        _enemiesRemaining.text = "Enemies Left : " + EnemiesAlive;

        // If the enemies are all dead then spawn the next wave 
        if (EnemiesAlive > 0)
        {
            return;
        }
        
        // If the last wave has been reached and the last level instance has been played through
        if (_waveIndex == _waves.Length)
        {
            // Next instance
            _instanceIndex++;
            _waveIndex = 0;
            
            if (_instanceIndex == _instances.Length)
            {
                _gameManager.WinLevel();
            }
        }
        
        // Start coroutine to spawn the waves of enemies (Always at least one wave)
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = _timeBetweenWaves;
            return;
        }

        // Reduce countdown by 1 every second
        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);

        _waveCountdownText.text = string.Format("{0:00.00}", _countdown);
    }

    private IEnumerator SpawnWave()
    {

        Wave wave = _waves[_waveIndex];

        EnemiesAlive = wave.Count;
        
        Instance instant = _instances[_instanceIndex];
        
        _currentSpawn = instant.SpawnPoint;
        CurrentTarget = instant.Target;
        _currentInstance.text = instant.InstanceName;
        
        for (int i = 0; i < wave.Count; i++)
        {
            
            SpawnEnemy(wave.Enemy);
            yield return new WaitForSeconds(1f/ wave.Count);
        }
        _waveIndex++;
    }

    public void SpawnEnemy(GameObject enemy)
    {
        var enemyUnitClone = Instantiate(enemy, _currentSpawn.position, _currentSpawn.rotation);
        enemyUnitClone.GetComponent<Unit>().Init(this);
        enemyUnitClone.GetComponent<Enemy>().Init(this);
    }
}
