using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    //[Header("Wave Editor")]
    [FormerlySerializedAs("_Levels")] [SerializeField] private Level[] _levels;
    
    // Storage Variables
    private int _enemiesAlive;
    private float _countdown = 2f;
    private Transform _currentSpawn;
    private GameObject _currentTarget;
    private bool _levelComplete;
    
    // Accessors 
    public int EnemiesAlive
    {
        get => _enemiesAlive;
        set => _enemiesAlive = value;
    }
    
    // Index counters for array(s)
    private int _levelIndex = 0;
    private int _waveIndex = 0;

    // UI Elements
    [FormerlySerializedAs("_waveCountdown")] [SerializeField] private Text waveCountdownText;
    [FormerlySerializedAs("_enemiesRemaining")] [SerializeField] private Text enemiesRemainingText;
    [FormerlySerializedAs("_currentLevel")] [SerializeField] private Text currentLevelText;

    // Game Manager (UI & Game-play)
    // [FormerlySerializedAs("_gameManager")] [SerializeField] private GameManager gameManager; 

    private void Start()
    {
//        print(_levels[0].Waves.Length);
//        print(_levels[0].Waves[0].Compositions.Length);
//        print(_levels[0].Waves[1].Compositions.Length);

        currentLevelText.text = "Level Beginning";
    }

    private void Update()
    {
        if (_levelComplete) return; // Game Complete
        if (GameManager.GameIsOver)
        {
            waveCountdownText.text = "...";
            return; // Game Over
        }

        enemiesRemainingText.text = "Enemies Left : " + _enemiesAlive;

        #region Countdown

        // Reduce countdown by 1 every second
        _countdown -= Time.deltaTime;
        // Make sure countdown is never less than 0
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = $"{_countdown:00.00}";

        #endregion
       
        if (_countdown <= 0f) waveCountdownText.text = "Final Wave";
        
        // Check if last level has been reached
        if (_levelIndex == _levels.Length)
        {
            //print("All Waves Spawned");
                
            // All Waves are spawned {Wait until all enemies are dead}
            // Or Player is killed
            if (_enemiesAlive > 0) return;
            LevelComplete();
            return;
        }
         
        // Start coroutine to spawn waves
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = _levels[_levelIndex].TimeBetweenWaves;
        }
    }
    
    private void LevelUpdate()
    { // Updates the Level 
        // If all waves in a level have been spawned - start next level
        if (_waveIndex != _levels[_levelIndex].Waves.Length) return;
        
        // Start from First wave
        _waveIndex = 0;
        // Start Next level
        _levelIndex++;
    }

    private void LevelComplete()
    {
        // ALl enemies killed - Level Complete!
        print("Level Complete");
        waveCountdownText.text = "Final Wave";
        _levelComplete = true;
    }
    
    private IEnumerator SpawnWave()
    {
        Level level = _levels[_levelIndex];
        PlayerStats.Rounds++;

        foreach (Composition x in level.Waves[_waveIndex].Compositions)
        {
            _enemiesAlive += x.Amount;
        }

        // Set level name
        currentLevelText.text = level.LevelName + " | Wave : " + (_waveIndex + 1);
        
        // Set Number of Enemies
        enemiesRemainingText.text = _enemiesAlive.ToString();
        
        // Set Start and End Points
        _currentSpawn = level.SpawnPoint;
        _currentTarget = level.Target;

        // Identify Current wave and spawn compositions :
        // Call current wave
        for (var x = _waveIndex; x <= _waveIndex; x++)
        {
            var comp = level.Waves[x].Compositions.Length; // 4
            // Get Number of compositions 
            for (var y = 0; y < comp; y++)
            {
                // Get Number of Enemies in composition
                for (var i = 0; i < level.Waves[x].Compositions[y].Amount; i++) {
                    // Spawn composition
                    SpawnEnemy(level.Waves[x].Compositions[y].EnemyType, _currentTarget);
                    
                    yield return new WaitForSeconds(level.Waves[x].Compositions[y].SpawnRate);
                }
                // Add delay between composition spawning
                yield return new WaitForSeconds(level.Waves[x].Compositions[y].NextDelay);
            }
        }
        _waveIndex++;
        LevelUpdate();
    }
    
    private void SpawnEnemy(GameObject enemy, GameObject target)
    {
        // Spawn Enemy and create reference to gameObject
        var currentEnemy = Instantiate(enemy, _currentSpawn.position, _currentSpawn.rotation);
        currentEnemy.GetComponent<EnemyMovement>().InvokeEnemy(this, target);
    }
}
