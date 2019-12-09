
using System;
using UnityEngine;

public class Pad : MonoBehaviour
{
    // Handle placement of turrets and user feedback, as well as if the player
    // has pressed used the pad / is it free or not?
    [SerializeField] Color hoverColor;
    [SerializeField] Vector3 positionOffSet;
    [SerializeField] Vector3 rotationOffset;
    
    //Check if turret is placed or not
    private GameObject _turret;
    
    // Reference to renderer component
    private Renderer _rend;
    private Color _startColour;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        _startColour = _rend.material.color;
    }

    private void OnMouseDown()
    {
        //Needs to be implemented onto phone
        if (_turret != null)
        {
            Debug.Log("Cannot build there! - TODO: put in canvas.");
            return;
        }
        
        // Build a turret
        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        _turret = (GameObject)Instantiate(turretToBuild,transform.position + positionOffSet, transform.rotation);
    }

    void OnMouseEnter()
    {
        //Needs to be implemented onto phone
        _rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        //Needs to be implemented onto phone
        _rend.material.color = _startColour;
    }
}
