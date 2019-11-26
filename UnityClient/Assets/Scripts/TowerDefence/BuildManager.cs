using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Make BuildManager available without a reference needed
    // [Only 1 instance of build manager in scene] | static - shared by all build managers
    public static BuildManager instance;

    void Awake()
    {
        // This is known as the singleton pattern, it ensures a class has only a single globally accessible instance available at all times
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;
        
    }

    public GameObject standardTurretPrefab;

    void Start()
    {
        turretToBuild = standardTurretPrefab;
    }

    // Turret selected by user
    private GameObject turretToBuild;
    
    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

}
