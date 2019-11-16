using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private bool GamePaused = false; /*Declare GamePaused variable, initially set to false*/

    [SerializeField]
    private GameObject pauseMenuUI;

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) return;

            if (GamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); /*Disable pause menu UI*/
        Time.timeScale = 1f; /*Set time in game to stop*/
        GamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); /*Enable pause menu UI*/
        Time.timeScale = 0f; /*Set time in game to stop*/
        GamePaused = true;
    }

}
