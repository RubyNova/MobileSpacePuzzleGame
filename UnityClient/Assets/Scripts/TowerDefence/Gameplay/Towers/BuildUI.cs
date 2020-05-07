using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject ui;

    [Header("Text UI")]
    public Text upgradeCost;
    public Text sellAmount;
    public Text repairAmount;

    public Text rangeAmount;
    public Text damageAmount;
    
    [Header("Button UI")]
    public Button upgradeButton;
    public Button repairButton;

    [Header("Image UI")] 
    [SerializeField] private Image healthBar;

    private BuildablePoint target;
    
    private void Update()
    {
        // Dont Check if there is no turret on the plot
        if (!target) return;
        if (!target.turret) return;
        
        var turretScript = target.turret.GetComponent<Turret>();

        if (!target.isDamaged)
        {
            repairAmount.text = "$0";
            repairButton.interactable = false;
            healthBar.fillAmount = 100;
            return;
        }
        
        // target is damaged allow repair button to be pressed
        repairButton.interactable = true;
        repairAmount.text = "$" + target.turretBlueprint.GetRepairAmount(turretScript.StartHealth, turretScript.Health);

        if (target.isDestroyed)
        { // target is destroyed disable upgrade button
            repairAmount.text = "$" + target.turretBlueprint.GetRepairAmount();
            upgradeButton.interactable = false;
            return;
        }
        
        
        healthBar.fillAmount = turretScript.Health / turretScript.StartHealth;
    }
    
    public void SetTarget(BuildablePoint point)
    { // Give information about selected turret via its buildable point
        target = point;

        transform.position = target.GetBuildPosition();
        
        // If turret is fully upgraded do not allow any further upgrades 
        if (!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "MAXED";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;

        if (target.turret)
        {
            var turretScript = target.turret.GetComponent<Turret>();
            var range = turretScript.GetRange();
            rangeAmount.text = range.ToString();
            damageAmount.text = turretScript.GetDamage();
        }

        ui.SetActive(true);
    }
    
    public void Hide()
    { // Hide UI
        ui.SetActive(false);
    }

    public void Upgrade()
    { // When Upgrading a turret
        target.UpgradeTurret();
        // Close menu on Upgrade
        BuildManager.instance.DeselectTurret();
    }

    public void Sell()
    { // When selling a turret
        target.SellTurret();
        BuildManager.instance.DeselectTurret();
    }

    public void Repair()
    { // When Repairing a destroyed Turret
        target.RepairTurret();
        BuildManager.instance.DeselectTurret();
    }
}
