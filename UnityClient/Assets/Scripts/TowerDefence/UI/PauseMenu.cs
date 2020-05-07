using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private bool _gamePaused = false; /*Declare GamePaused variable, initially set to false*/

    [SerializeField]
    private GameObject _pauseMenuUI;


    public void Resume()
    {
        _pauseMenuUI.SetActive(false); /*Disable pause menu UI*/
        Time.timeScale = 1f; /*Set time in game to stop*/
        _gamePaused = false;
    }


    public void Pause()
    {
        _pauseMenuUI.SetActive(true); /*Enable pause menu UI*/
        Time.timeScale = 0f; /*Set time in game to stop*/
        _gamePaused = true;
    }

}
