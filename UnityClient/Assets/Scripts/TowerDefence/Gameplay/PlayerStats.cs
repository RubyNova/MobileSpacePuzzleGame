using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Currency Variables
    public static int Money;
    [SerializeField] private int startMoney = 400;
    
    // Player Lives Variables
    public static int Lives;
    [SerializeField] private int startLives = 20;
    
    // Rounds Survived
    public static int Rounds;

    void Start()
    {
        Money = startMoney;
        Lives = startLives;

        Rounds = 0;
    }
}
