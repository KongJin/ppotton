using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Fusion.Addons.FSM;
using UnityEngine;

public class AttackBehaviour : AdvancedStateBehaviour
{   

    [SerializeField]
    private PrepareState _prepareState;
    [SerializeField]
    private AttackState _attackState;
    [SerializeField]
    private RecoverState _recoverState;

    private StateMachine<State> _attackMachine;


    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _attackMachine = new StateMachine<State>("Attack Machine", _prepareState, _attackState, _recoverState);
        stateMachines.Add(_attackMachine);
    }

    protected override void OnEnterState()
    {
        // Reset to Prepare state
        _attackMachine.ForceActivateState(_prepareState, true);
        customMachine. Animancer.SetTrigger("attack");
    }

    protected override void OnEnterStateRender()
    {
        
    }

    
    protected override void OnFixedUpdate()
    {
        if (_recoverState.IsFinished == true)
        {
            // Attack finished, deactivate
            Machine.TryDeactivateState(StateId);
        }
    }

    // STATES

    [Serializable]
    public class PrepareState : State
    {
        protected override void OnFixedUpdate()
        {
            if (Machine.StateTime > 0.5f)
            {
                Machine.TryActivateState<AttackState>();
            }
        }

        protected override void OnEnterStateRender()
        {
            Debug.Log("Preparing attack...");
        }
    }

    [Serializable]
    public class AttackState : State
    { 
        protected override void OnFixedUpdate()
        {
            if (Machine.StateTime > 0.5f)
            {
                Machine.TryActivateState<RecoverState>();
            }
        }

        protected override void OnEnterStateRender()
        {
            Debug.Log("Attacking...");
        }
    }

    [Serializable]
    public class RecoverState : State
    {
        public bool IsFinished => Machine.ActiveStateId == StateId && Machine.StateTime > 2f;

        protected override void OnEnterStateRender()
        {
            Debug.Log("Recovering from attack...");
        }
    }
}