using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildablePoint : MonoBehaviour
{
    private Color startColor;
    private Color turretRepairColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Color burntColor;
    
    [SerializeField] private Vector3 positionOffset;

    // Can also allow pre-made turrets on levels
    [HideInInspector] public GameObject turret;
    [HideInInspector] public TurretBlueprint turretBlueprint;
    [HideInInspector] public bool isUpgraded = false;
    
    // Handle Turret health and states
    [HideInInspector] public bool isDamaged = false;
    [HideInInspector] public bool isDestroyed = false;

    private Renderer rend;
    private BuildManager buildManager;
    
    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }
    
    public void DestroyTurret()
    {
        // Get materials main color
        var turretRender = turret.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material;

        // Apply burnt color
        turretRender.color = burntColor;
        // Stop Damage Check
        CancelInvoke(nameof(CheckForDamage));
    }
    
    private void CheckForDamage()
    {
        if (!turret) return; // if there is no turret placed return

        var turretScript = turret.GetComponent<Turret>();

        if (turretScript.Damaged) isDamaged = true;
        if (turretScript.Destroyed) isDestroyed = true;
        
        // If destroyed
        if (!isDestroyed) return;
        // Replace with destroyed Turret Prefab

        BurntTurret();

        DestroyTurret();
    }
    
    private void BurntTurret()
    {
        // Get materials main color
        var turretRender = turret.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material;
        // Save Turret Color for when it is repaired
        turretRepairColor = turretRender.color;
    }
    
    private void BuildTurret(TurretBlueprint blueprint)
    {
        // Check Player has enough Currency
        if (PlayerStats.Money < blueprint.cost)
        {
            print("Not Enough Money to Build");
            return;
        }

        // Repeating Function checking turrets health
        InvokeRepeating(nameof(CheckForDamage), 2f, 0.2f);

        // Take away money
        PlayerStats.Money -= blueprint.cost;
        
        // Build Turret
        GameObject _turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        
        // Set blueprint of turret to passed in turret for upgrade / sell
        turretBlueprint = blueprint;

        // Play build effect
        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }
    
    public void UpgradeTurret()
    {
        // Check Player has enough Currency
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            print("Not Enough Money to Upgrade");
            return;
        }

        // Take away money
        PlayerStats.Money -= turretBlueprint.upgradeCost;

        // Remove old turret
        Destroy(turret);

        // Build Upgraded Turret
        GameObject _turret = Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret; 

        // Play build effect
        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true;
        
        print("Turret Upgraded");
    }
    
    public void SellTurret()
    {
        // Refund Player some Money
        PlayerStats.Money += turretBlueprint.GetSellAmount();
        
        // Stop Repeating Function checking turrets health
        CancelInvoke(nameof(CheckForDamage));

        //Spawn Effect
        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
        
        Destroy(turret);
        turretBlueprint = null;

        isDamaged = false;
        isDestroyed = false;
    }
    
    public void RepairTurret()
    {
        if (!isDamaged)
        { // If turret is not damaged
            print("Turret is not damaged");
            return;
        }

        // Check Player has enough Currency
        if (PlayerStats.Money < turretBlueprint.GetRepairAmount())
        {
            print("Not Enough Money to Upgrade");
            return;
        }
        
        // Get Turret component 
        var turretScript = turret.GetComponent<Turret>();

        if (isDestroyed)
        {
            // Take away money 
            PlayerStats.Money -= turretBlueprint.GetRepairAmount();
            print("That full repair cost $" +
                  turretBlueprint.GetRepairAmount());
        }
        else if (isDamaged)
        {
            PlayerStats.Money -= turretBlueprint.GetRepairAmount(turretScript.StartHealth, turretScript.Health);
            print("That partial repair cost $" +
                  turretBlueprint.GetRepairAmount(turretScript.StartHealth, turretScript.Health));
        }
        
        // Repair Turret
        turretScript.RepairTurret();

        isDamaged = false;

        if (!isDestroyed) return;
        
        isDestroyed = false;
        
        // Change Materials to turret Color
        turret.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.color = turretRepairColor;

        // Repeating Function checking turrets health
        InvokeRepeating(nameof(CheckForDamage), 2f, 0.2f);
    }
    
    private void OnMouseDown()
    {
        // Stop placing of turrets if plot is below UI
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Turret already built
        if (turret != null)
        {
            buildManager.SelectTurretToEdit(this);
            return;
        }

        // Check if there is no turret selected
        if (!buildManager.CanBuild) 
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }
    
    private void OnMouseEnter()
    {
        // Stop placing of turrets if plot is below UI
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        // Check if there is no turret selected
        if (!buildManager.CanBuild) 
            return;

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        }
        else rend.material.color = notEnoughMoneyColor;
    }
    
    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
