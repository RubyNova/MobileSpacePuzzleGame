using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maneuver : MonoBehaviour
{
    private Unit unit;
    private Enemy enemy;

    private SphereCollider collider;
    private Renderer rend;

    private Vector3 manoeuvorTo;
    private bool movingAround;
    
    private void Awake()
    {
        // Get Parent Units Components References
        enemy = GetComponentInParent<Enemy>();
        unit = GetComponentInParent<Unit>();

        collider = GetComponent<SphereCollider>();
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If it isn't a unit continue
        if (!other.GetComponent<Unit>()) return;
        if (other.gameObject.name != "Pool(Clone)") return;
        if (movingAround) return;

        gameObject.GetComponentInParent<Renderer>().material.color = new Color32(185,46,52,0);

        unit.GetComponent<Rigidbody>().detectCollisions = false;
        movingAround = true;
        print("Activated");

        StartCoroutine(nameof(PassThrough));
    }
    
    private IEnumerator PassThrough()
    {
        yield return new WaitForSeconds(2f);
        print("Reactivated");
        unit.GetComponent<Rigidbody>().detectCollisions = true;
        movingAround = false;
    }
}
