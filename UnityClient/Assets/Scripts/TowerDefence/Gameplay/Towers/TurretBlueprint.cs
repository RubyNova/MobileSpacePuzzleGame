using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    [Header("General")] 
    public GameObject prefab;
    public int cost;
    public GameObject destroyedPrefab;

    [SerializeField] [Range(1, 100)] private int repairPercentage = 60;

    [Header("Turret Upgrade")] public GameObject upgradedPrefab;
    public int upgradeCost;
    public GameObject destroyedUpgradedPrefab;

    public int GetSellAmount()
    {
        return cost / 2;
    }

    public int GetRepairAmount()
    {
        // Return Repair Cost
        return cost / 100 * repairPercentage; // 60% of cost to repair Turret
    }

    public int GetRepairAmount(float startHealth, float health)
    {
        // If turret is only partially damaged return Cost of only that
        var fullRepairCost = cost / 100 * repairPercentage;
        var percentageOfHealthLeft = 100 / startHealth * health; // Get Number from 0% - 100%

        var returnable = fullRepairCost * percentageOfHealthLeft / 100;
        return Convert.ToInt32(returnable);
    }
}
