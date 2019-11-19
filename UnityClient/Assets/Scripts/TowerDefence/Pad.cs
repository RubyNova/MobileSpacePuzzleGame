
using System;
using UnityEngine;

public class Pad : MonoBehaviour
{
    // Handle placement of turrets and user feedback, as well as if the player
    // has pressed used the pad / is it free or not?
    public Color hoverColor;
    public Vector3 positionOffSet;
    public Vector3 rotationOffset;
    
    //Check if turret is placed or not
    private GameObject turret;
    
    // Reference to renderer component
    private Renderer rend;
    private Color startColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void OnMouseDown()
    {
        //Needs to be implemented onto phone
        if (turret != null)
        {
            Debug.Log("Cannot build there! - TODO: put in canvas.");
            return;
        }
        
        // Build a turret
        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        turret = (GameObject)Instantiate(turretToBuild,transform.position + positionOffSet, transform.rotation);
    }

    void OnMouseEnter()
    {
        //Needs to be implemented onto phone
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        //Needs to be implemented onto phone
        rend.material.color = startColor;
    }
}
