using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.FSM;
using Fusion.Addons.KCC;
using UnityEngine;

public interface ICustomMachine
{
    NetworkMecanimAnimator Animancer { get; }
    KCC Kcc { get; }
}
public abstract class AdvancedStateBehaviour : StateBehaviour<AdvancedStateBehaviour>
{
    public  ICustomMachine customMachine { protected get; set; }

    [SerializeField] protected AnimationClip _clip;
}


public abstract class AdvancedState : State<AdvancedState>
{
    protected ICustomMachine customMachine;

    public bool IsDone { get; protected set; } = false;
    public AdvancedState(ICustomMachine machine)
    {
        this.customMachine = machine;
    }
    protected override void OnExitState()
    {
        base.OnExitState();
        IsDone = false;
    }

}