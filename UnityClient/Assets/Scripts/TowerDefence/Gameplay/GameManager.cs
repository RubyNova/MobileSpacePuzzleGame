using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;
    public static bool WaveSpawned;
    
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject startWaveEarlyUi;
    
    private void Start()
    {
        // Called start of every Scene
        GameIsOver = false;
        WaveSpawned = false;
    }
    
    void Update()
    {
        if (GameIsOver) return;
        
        // If wave is spawned Show UI to start next wave
        if (WaveSpawned) startWaveEarlyUi.SetActive(true);
        else if (!WaveSpawned) startWaveEarlyUi.SetActive(false);

        if (PlayerStats.Lives <= 0) EndGame();
    }

    private void EndGame()
    {
        GameIsOver = true;
        print("Game Over!");
        
        gameOverUi.SetActive(true);
    }
}
