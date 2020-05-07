using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFSM : StateMachineBehaviour
{
    public GameObject thisTurret;
    public GameObject partToRotate;
    public Turret turretScript;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thisTurret = animator.gameObject;
        turretScript = thisTurret.GetComponent<Turret>();
        partToRotate = thisTurret.transform.GetChild(0).gameObject;
    }
}
