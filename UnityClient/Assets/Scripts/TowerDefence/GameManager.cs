using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject completeLevelUI;

    public void WinLevel()
    {
        completeLevelUI.SetActive(true);
    }
}
