using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;
    
    [SerializeField] private GameObject gameOverUI;
    
    private void Start()
    {
        // Called start of every Scene
        GameIsOver = false;
    }
    
    void Update()
    {
        if (GameIsOver) return;

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        GameIsOver = true;
        print("Game Over!");
        
        gameOverUI.SetActive(true);
    }
}
