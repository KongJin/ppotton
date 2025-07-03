using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Fusion.Addons.FSM;
using UnityEngine;

public class IdleBehaviour : AdvancedStateBehaviour
{
    [SerializeField]
    private PrepareState _prepareState;
    [SerializeField]
    private IdleState _attackState;
    [SerializeField]
    private RecoverState _recoverState;

    private StateMachine<State> _attackMachine;


    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _attackMachine = new StateMachine<State>("Walk Machine", _prepareState, _attackState, _recoverState);
        stateMachines.Add(_attackMachine);
    }

    protected override void OnEnterState()
    {        
        _attackMachine.ForceActivateState(_prepareState, true);       
    }
    protected override void OnEnterStateRender()
    {
         //_animancer.Play(_clip);
    }

    protected override void OnFixedUpdate()
    {
        if (Machine == null)
        {
            Debug.Log("Machine is null");
            return;
        }

        if (_recoverState.IsFinished == true)
        {
            // Attack finished, deactivate
            Machine.TryDeactivateState(StateId);
        }
    }

    // STATESc

    [Serializable]
    public class PrepareState : State
    {
        protected override void OnFixedUpdate()
        {
            if (Machine.StateTime > 1f)
            {
                Machine.TryActivateState<IdleState>();
            }
        }

        protected override void OnEnterStateRender()
        {
            Debug.Log("Preparing Idle...");
        }
    }

    [Serializable]
    public class IdleState : State
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
            Debug.Log("Idling...");
        }
    }

    [Serializable]
    public class RecoverState : State
    {
        public bool IsFinished => Machine.ActiveStateId == StateId && Machine.StateTime > 2f;

        protected override void OnEnterStateRender()
        {
            Debug.Log("Recovering from Idle...");
        }
    }
}
