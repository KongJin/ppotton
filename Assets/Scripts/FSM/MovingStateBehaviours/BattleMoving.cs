using System;
using System.Collections;
using System.Collections.Generic;
using Fusion.Addons.FSM;
using Fusion.Addons.KCC;
using UnityEngine;

public class BattleMoving : AdvancedStateBehaviour
{
    Animator _anim;
    Transform _reporter;
    KCC kcc;
    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _reporter = Repoter.Instance.transform;
        _anim = customMachine.Animancer.Animator;
        kcc = customMachine.Kcc;
    }
    protected override void OnEnterState()
    {
        _anim.SetBool("battle", true);
    }
    protected override void OnFixedUpdate()
    {
        Quaternion targetRot = Quaternion.LookRotation(_reporter.forward);
        Quaternion currentRot = kcc.Data.LookRotation;
        float t = PlayerInputContainer.Instance.DeltaTime * 5f;
        Quaternion smoothed = Quaternion.Slerp(currentRot, targetRot, t);
        kcc.SetLookRotation(smoothed);
        if (kcc.Data.IsGrounded) kcc.SetInputDirection(PlayerInputContainer.Instance.Movement * 0.5f);
        var movement = _reporter.InverseTransformDirection(PlayerInputContainer.Instance.Movement);
        _anim.SetFloat("movement_battle_H", movement.x);
        _anim.SetFloat("movement_battle_V", movement.z);
    }
  
}
