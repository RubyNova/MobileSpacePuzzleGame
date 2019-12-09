using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("completeLevelUI")] [SerializeField] private GameObject _completeLevelUI;

    public void WinLevel()
    {
        _completeLevelUI.SetActive(true);
        gameComplete();
    }

    void gameComplete()
    {
        Time.timeScale = 0;
    }
}
