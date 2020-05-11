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
    
    // Stop and Start of Waves
    // Allow player to start wave early 
    private bool currentWaveSpawned;
    
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

    [SerializeField] private int startingBonus;
    private int currentBonus;
    
    [SerializeField] private Text moneyText;
    [SerializeField] private Image startWaveCountdown;
    private float timeAtWaveCompletion;
    
    // Game Manager (UI & Game-play)
    // [FormerlySerializedAs("_gameManager")] [SerializeField] private GameManager gameManager; 

    private void Start()
    {
//        print(_levels[0].Waves.Length);
//        print(_levels[0].Waves[0].Compositions.Length);
//        print(_levels[0].Waves[1].Compositions.Length);

        currentLevelText.text = "Level Beginning";
        ResetNextWaveButton();
        
        // Initial wave start
        _countdown = 1;
    }
    
    private void ResetNextWaveButton()
    {
        currentBonus = startingBonus;
        moneyText.text = "$" + startingBonus;
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
        
        // Visual Countdown on NextWave button
        startWaveCountdown.fillAmount = _countdown / timeAtWaveCompletion;
        
        var part = timeAtWaveCompletion / 4;

        if (_countdown > part * 3)
        {
            currentBonus = startingBonus;
            moneyText.text = startingBonus.ToString();
        }
        else if (_countdown > part * 2)
        {
            currentBonus = startingBonus / 4 * 3;
            moneyText.text = currentBonus.ToString();
        }
        else if (_countdown > part * 1)
        {
            currentBonus = startingBonus / 4 * 2;
            moneyText.text = currentBonus.ToString();
        }
        else if (_countdown > part * 0.2)
        {
            currentBonus = startingBonus / 4 * 1;
            moneyText.text = currentBonus.ToString();
        }
        else
        {
            moneyText.text = "$0";
            currentBonus = 0;
        }

        #endregion
       
        if (_countdown <= 0f) waveCountdownText.text = "Final Wave";
        
        // Check if last level has been reached
        if (_levelIndex == _levels.Length)
        {
            //print("All Waves Spawned");
            GameManager.WaveSpawned = false;
                
            // All Waves are spawned {Wait until all enemies are dead}
            // Or Player is killed
            if (_enemiesAlive > 0) return;
            LevelComplete();
            return;
        }
         
        // Start coroutine to spawn waves
        if (_countdown <= 0f)
        {
            // Wave spawned turned to false
            if (currentWaveSpawned)
            {
                currentWaveSpawned = false;
                GameManager.WaveSpawned = false;
            }
            
            StartCoroutine(SpawnWave());
            _countdown = _levels[_levelIndex].TimeBetweenWaves;
        }
    }
    
    public void StartWaveEarly()
    {
        if (!currentWaveSpawned) return;

        // Give Player extra money
        PlayerStats.Money += currentBonus;

        // Reset Wave button
        ResetNextWaveButton();

        // Start wave by resetting wave countdown
        _countdown = 0;
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
                // Set level name
                currentLevelText.text = level.LevelName + " | Wave : " + (_waveIndex + 1) + "| Comp " +  (y + 1);

                // Get Number of Enemies in composition
                for (var i = 0; i < level.Waves[x].Compositions[y].Amount; i++) {
                    // Spawn composition
                    SpawnEnemy(level.Waves[x].Compositions[y].EnemyType, _currentTarget);
                    
                    yield return new WaitForSeconds(level.Waves[x].Compositions[y].SpawnRate);
                }
                // Add delay between composition spawning
                yield return new WaitForSeconds(level.Waves[x].Compositions[y].NextDelay);
            }
            // Wave spawned turned to true
            currentWaveSpawned = true;
            GameManager.WaveSpawned = true;
            timeAtWaveCompletion = _countdown;
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
