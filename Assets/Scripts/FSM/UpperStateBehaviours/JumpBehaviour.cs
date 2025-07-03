using System;
using System.Collections;
using System.Collections.Generic;
using Fusion.Addons.FSM;
using Fusion.Addons.KCC;

using UnityEngine;

public class JumpBehaviour : AdvancedStateBehaviour
{


    private StateMachine<AdvancedState> _jumpMachine;

    private RecoverState _recoverState;
    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _recoverState = new RecoverState(customMachine);
        _jumpMachine = new StateMachine<AdvancedState>("Jump Machine",
            new PrepareState(customMachine), new DoingState(customMachine), _recoverState);
        stateMachines.Add(_jumpMachine);
    }

    protected override void OnEnterState()
    {
        // Reset to Prepare state
        _jumpMachine.ForceActivateState<PrepareState>(true);

        //_characterController.Move(Vector3.up * 500);
    }
    protected override void OnEnterStateRender()
    {
        customMachine.Animancer.SetTrigger("jump");

    }


    protected override bool CanExitState(AdvancedStateBehaviour nextState)
    {
        //return _jumpMachine.ActiveState is RecoverState;
        return _jumpMachine.ActiveState is RecoverState ;
    }

    protected override void OnFixedUpdate()
    {       
        if (_recoverState.IsFinished)
        {            
            Machine.TryDeactivateState(StateId);
        }
    }

    // STATES

    [Serializable]
    public class PrepareState : AdvancedState
    {
        public PrepareState(ICustomMachine cMachine) : base(cMachine) { }
        protected override void OnFixedUpdate()
        {
            if (Machine.StateTime > 0.75f)
            {
                Machine.TryActivateState<DoingState>();

            }
        }

        protected override void OnEnterStateRender()
        {
            Debug.Log("Preparing Jump...");
        }
    }

    [Serializable]
    public class DoingState : AdvancedState
    {
        KCC _kcc;
        public DoingState(ICustomMachine cmachine):base(cmachine)
        {
            _kcc = cmachine.Kcc;
        }
        protected override void OnFixedUpdate()
        {
            if (Machine.StateTime > 0.5f)
            {
                Machine.TryActivateState<RecoverState>();
            }
        }
        protected override void OnEnterState()
        {
            if (customMachine.Kcc.Data.IsGrounded == false) return;
            _kcc.Jump(Vector3.up *5);            
            _kcc.SetDynamicVelocity(PlayerInputContainer.Instance.Movement);            
        }
        protected override void OnEnterStateRender()
        {
            Debug.Log("Jumping...");            
        }
    }

    [Serializable]
    public class RecoverState : AdvancedState
    {
        public RecoverState(ICustomMachine cMachine) : base(cMachine) { }
        public bool IsFinished { get; private set; }

        protected override void OnExitState()
        {
            base.OnExitState();
            customMachine.Animancer.Animator.speed = 1;
        }
        protected override void OnFixedUpdate()
        {
            if (customMachine.Kcc.Data.IsGrounded)
            {
                customMachine.Animancer.Animator.speed = 1;
                IsFinished = true;
            }
        }
        protected override void OnEnterState()
        {
            IsFinished = false;
            Debug.Log("Recovering from Jump...");

            //customMachine.Animancer.Animator.speed = 0.1f;
            customMachine .Kcc.SetDynamicVelocity(Vector3.down);
        }
    }
}
