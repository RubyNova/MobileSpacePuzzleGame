using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint rocketTurret;
    public TurretBlueprint iceTurret;
    public TurretBlueprint fireTurret;
        
    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectStandardTurret()
    {
        print("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }
    
    public void SelectRocketTurret()
    {
        print("Rocket Turret Selected");
        buildManager.SelectTurretToBuild(rocketTurret);
    }    
    
    public void SelectIceTurret()
    {
        print("Ice Turret Selected");
        buildManager.SelectTurretToBuild(iceTurret);
    }
    
    public void SelectFireTurret()
    {
        print("Fire Turret Selected");
        buildManager.SelectTurretToBuild(fireTurret);
    }
}
