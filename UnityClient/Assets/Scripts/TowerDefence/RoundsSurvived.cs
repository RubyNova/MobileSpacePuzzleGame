using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundsSurvived : MonoBehaviour
{
    [SerializeField] Text roundsText;
    int round = 0;
    
    void Update()
    {
        if (round < PlayerStats.Rounds)
        {
            round++;
            roundsText.text = "round " + round;
        }
    }
}
