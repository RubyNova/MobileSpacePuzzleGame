using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    private Unit unit;
    private Enemy enemy;
        
    private GameObject otherUnit;
        
    private bool triggered;
    private float maximumSpeed;
    
    private void Awake()
    {
        // Get Parent Units Components References
        enemy = GetComponentInParent<Enemy>();
        unit = GetComponentInParent<Unit>();
        
        maximumSpeed = enemy.Speed;
    }
    
    private void FixedUpdate()
    {
        if (!triggered) return;

        if (otherUnit == null)
        {
            UnitDestroyed();
            return;
        }
        
        if (enemy.Speed < 0) return;

        enemy.Speed = 0f;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // If it isn't a unit continue
        if (!other.GetComponent<Unit>()) return;

        otherUnit = other.gameObject;
        triggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // If it isn't a unit continue
        if (!other.GetComponent<Unit>()) return;
        enemy.Speed = maximumSpeed;
        triggered = false;
    }

    private void UnitDestroyed()
    {
        enemy.Speed = maximumSpeed;
        triggered = false;
    }
}
