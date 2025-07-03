using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Fusion.Addons.FSM;
using UnityEngine;
using Fusion;


//LinearMixerTransition 가 animancerPro에서만 사용 가능하여 폐기
public class MovementBehaviour : AdvancedStateBehaviour 
{
    [SerializeField]
    private PrepareState _prepareState;
    [SerializeField]
    private IdleState _attackState;
    [SerializeField]
    private RecoverState _recoverState;


    private StateMachine<State> _attackMachine;
    
    public static float BlendingValue { get; private set; }

    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _attackMachine = new StateMachine<State>("Movement Machine", _prepareState, _attackState, _recoverState);
        stateMachines.Add(_attackMachine);
    }

    protected override void OnEnterState()
    {
        _attackMachine.ForceActivateState(_prepareState, true);

    }
    protected override void OnEnterStateRender()
    {
        
        Debug.Log("OnEnterStateRender");
    }


    protected override void OnFixedUpdate()
    {
        //_State.Parameter = PlayerAnimController.MovementLerp.magnitude;
        
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
        }
    }

    [Serializable]
    public class RecoverState : State
    {
        public bool IsFinished => Machine.ActiveStateId == StateId && Machine.StateTime > 2f;

        protected override void OnEnterStateRender()
        {
            
        }
    }
}
