using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : TurretFSM
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => base.OnStateEnter(animator, stateInfo, layerIndex);
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (turretScript.Target) return;
        
        // Play Idle Animation
    }
}
