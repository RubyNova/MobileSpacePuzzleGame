using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private bool GamePaused = false; /*Declare GamePaused variable, initially set to false*/

    [SerializeField]
    private GameObject pauseMenuUI;


    private void Resume()
    {
        pauseMenuUI.SetActive(false); /*Disable pause menu UI*/
        Time.timeScale = 1f; /*Set time in game to stop*/
        GamePaused = false;
    }


    private void Pause()
    {
        pauseMenuUI.SetActive(true); /*Enable pause menu UI*/
        Time.timeScale = 0f; /*Set time in game to stop*/
        GamePaused = true;
    }

}
