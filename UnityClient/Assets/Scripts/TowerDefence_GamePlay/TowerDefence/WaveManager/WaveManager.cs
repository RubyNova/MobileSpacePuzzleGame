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
    private bool LevelComplete;
    
    // Accessors 
    public int EnemiesAlive
    {
        get => _enemiesAlive;
        set => _enemiesAlive = value;
    }
    
    // Index counters for array(s)
    private int _levelIndex = 0;
    private int _waveIndex = 0;
    private int _compositionIndex = 0;
    
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
    }

    private void Update()
    {
        if (LevelComplete) return;

        // Show enemies Remaining
        if (enemiesRemainingText.text == string.Empty) enemiesRemainingText.text = "Enemies Left : " + _enemiesAlive;

        // Check if enemies are all dead [return if they aren't]
        if (_enemiesAlive > 0)
        {
            enemiesRemainingText.text = "Enemies Left : " + _enemiesAlive;
            return;
        }

        if (_enemiesAlive == 0) enemiesRemainingText.text = "0";

        // If the last level has been reached and all levels have been cleared
        if (_waveIndex == _levels[_levelIndex].Waves.Length)
        {
            _waveIndex = 0;
            // Move onto next level
            _levelIndex++;

            if (_levelIndex == _levels.Length)
            {
                print("Level Complete");

                LevelComplete = true;
                return;
            }
        }


        // Start coroutine to spawn waves
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = _levels[_levelIndex].TimeBetweenWaves;
            return;
        }

        // Reduce countdown by 1 every second
        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = $"{_countdown:00.00}";
    }
    
    private IEnumerator SpawnWave()
    {
        Level level = _levels[_levelIndex];

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
    }
    
    private void SpawnEnemy(GameObject enemy, GameObject target)
    {
        // Spawn Enemy and create reference to gameObject
        var currentEnemy = Instantiate(enemy, _currentSpawn.position, _currentSpawn.rotation);
        currentEnemy.GetComponent<Unit>().InvokeEnemy(this, target);
    }
    
    [ContextMenu("Print Indexes")]
    private void PrintIndexes()
    {

        var x = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject a in x)
        {
            var temp = a.GetComponent<Unit>();
            temp.TakeDamage(100f);
        }
    }
}
