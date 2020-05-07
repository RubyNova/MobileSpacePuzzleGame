using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    #region Singleton
    
    // Singleton pattern to get 1 instance of this manager at any one time
    public static BuildManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) return; // Been set before
        instance = this;
    }    
    
    #endregion

    public GameObject buildEffect;
    public GameObject sellEffect;

    private TurretBlueprint turretToBuild;
    private BuildablePoint selectedTurret;
    
    [SerializeField] private BuildUI buildUi;
    
    // Only allow anything to get this variable
    public bool CanBuild => turretToBuild != null;
    public bool HasMoney => PlayerStats.Money >= turretToBuild.cost;

    public void SelectTurretToEdit(BuildablePoint point)
    { // Editing a existing turret in play
        
        // If node already selected - toggle off
        if (selectedTurret == point)
        {
            DeselectTurret();
            return;
        }
        
        selectedTurret = point;
        turretToBuild = null;
        
        // Reference to the Build UI -- pass in point selected
        buildUi.SetTarget(point);
    }
    
    public void DeselectTurret()
    { // Deselect selected Turret
        selectedTurret = null;
        buildUi.Hide();
    }
    
    public void SelectTurretToBuild(TurretBlueprint turret)
    { // Building a new Turret from Shop
        turretToBuild = turret;
        
        DeselectTurret();
    }
    
    public TurretBlueprint GetTurretToBuild()
    { // Return turret to build
        return turretToBuild;
    }
}
