
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Pad : MonoBehaviour
{
    // Handle placement of turrets and user feedback, as well as if the player
    // has pressed used the pad / is it free or not?
    [FormerlySerializedAs("hoverColor")] [SerializeField] private Color _hoverColor;
    [FormerlySerializedAs("positionOffSet")] [SerializeField] private Vector3 _positionOffSet;
    [FormerlySerializedAs("rotationOffset")] [SerializeField] private Vector3 _rotationOffset;
    
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
        _turret = (GameObject)Instantiate(turretToBuild,transform.position + _positionOffSet, transform.rotation);
    }

    void OnMouseEnter()
    {
        //Needs to be implemented onto phone
        _rend.material.color = _hoverColor;
    }

    private void OnMouseExit()
    {
        //Needs to be implemented onto phone
        _rend.material.color = _startColour;
    }
}
