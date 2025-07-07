using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.FSM;
using Fusion.Addons.KCC;
using UnityEngine;

public class NormalMoving : AdvancedStateBehaviour
{
    Vector3 movementPrevious;
    Transform _reporter;
    Animator _anim;
    KCC kcc;
    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _reporter = Repoter.Instance.transform;
        _anim = customMachine.Animancer.Animator;
        kcc = customMachine.Kcc;
    }

    protected override void OnEnterState()
    {
        _anim.SetBool("battle", false);
    }
    protected override void OnFixedUpdate()
    {
        movementPrevious = PlayerInputContainer.Instance.Movement;        
        kcc.SetInputDirection(PlayerInputContainer.Instance.Movement*0.8f);
        if (movementPrevious != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(movementPrevious);
            Quaternion currentRot = kcc.Data.LookRotation;
            float t = PlayerInputContainer.Instance.DeltaTime * 5f;
            Quaternion smoothed = Quaternion.Slerp(currentRot, targetRot, t);
            kcc.SetLookRotation(smoothed);
        }
        
        _anim.SetFloat("movement_normal", movementPrevious.magnitude);
    }
}
