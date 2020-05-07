using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreDamage : MonoBehaviour
{
    private Unit unit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Unit>()) return;
        
        unit = other.GetComponent<Unit>();
        // Also update Wave Manager counter
        print(other.gameObject.name + " Reached the Core");
        unit.ReachedCore = true;
        unit.Die();
        
        // Take away a life from the player
        PlayerStats.Lives--;
    }
}
