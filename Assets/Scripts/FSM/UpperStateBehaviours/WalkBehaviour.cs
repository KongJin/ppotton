using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Fusion.Addons.FSM;
using UnityEngine;

public class WalkBehaviour : AdvancedStateBehaviour
{
    [SerializeField]
    private PrepareState _prepareState;
    [SerializeField]
    private IdleState _attackState;
    [SerializeField]
    private RecoverState _recoverState;

    private StateMachine<State> _walkMachine;


    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _walkMachine = new StateMachine<State>("Walk Machine", _prepareState, _attackState, _recoverState);
        stateMachines.Add(_walkMachine);
    }

    protected override void OnEnterState()
    {
        // Reset to Prepare state
        Debug.Log("Walk OnEnterState");
        _walkMachine.ForceActivateState(_prepareState, true);
               
    }

    protected override void OnEnterStateRender()
    {
        //_animancer.Play(_clip);
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
            if (Machine.StateTime > 1f)
            {
                Machine.TryActivateState<IdleState>();
            }
        }

        protected override void OnEnterStateRender()
        {
            Debug.Log("Preparing Walk...");
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
            Debug.Log("Walking...");
        }
    }

    [Serializable]
    public class RecoverState : State
    {
        public bool IsFinished => Machine.ActiveStateId == StateId && Machine.StateTime > 2f;

        protected override void OnEnterStateRender()
        {
            Debug.Log("Recovering from Walk...");
        }
    }
}
