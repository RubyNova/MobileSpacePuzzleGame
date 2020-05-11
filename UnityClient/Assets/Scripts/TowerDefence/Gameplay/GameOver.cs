using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text roundsText;

    [SerializeField] private GameObject canvasToHide;

    private void OnEnable()
    { // Called when Object is enabled
        roundsText.text = PlayerStats.Rounds.ToString();

        canvasToHide.SetActive(false);
    }

    public void Retry()
    { // 0 is the index that the main scene has been given in build settings - Needs to be pre-set
        // SceneManager.LoadScene(0);
      
        // -- When Reloading scene the lighting does not load due to a Unity specific behaviour
        // To fix go to -> Window/Lighting -> Scroll down and deselect Auto Generate and Bake Lighting
        // Only do this at the end of development - Auto gen needs to be on whilst editing the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        print("Go To Menu.");
    }
}
