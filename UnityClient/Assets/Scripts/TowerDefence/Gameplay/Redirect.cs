using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Redirect : MonoBehaviour
{
    [SerializeField] private GameObject shortcut;
    [SerializeField] private bool available = true;

    private Unit unit;
    private EnemyMovement movement;

    private void OnTriggerEnter(Collider other)
    {
        if (!available) return;
        //if (other.gameObject.name != "Unit(Clone)") return;
        if (!other.GetComponent<Enemy>()) return;
        if (name == "Start")
        {
            if (!ChanceOfUsingShortCut()) return;
        }
        print("contains" + other.GetComponent<EnemyMovement>().GetTarget());
        movement = other.GetComponent<EnemyMovement>();
        movement.ShortCut(shortcut);
    }

    private bool ChanceOfUsingShortCut()
    { // 50% chance to take shortcut
        int randomNumber = Random.Range(0, 2);
        print(randomNumber);
        return randomNumber == 1;
    }
}
