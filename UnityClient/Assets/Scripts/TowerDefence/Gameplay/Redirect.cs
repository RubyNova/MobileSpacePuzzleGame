using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (!other.GetComponent<Unit>()) return;
        print("contains" + other.GetComponent<EnemyMovement>().GetTarget());
        movement = other.GetComponent<EnemyMovement>();
        movement.ShortCut(shortcut);
    }
}
