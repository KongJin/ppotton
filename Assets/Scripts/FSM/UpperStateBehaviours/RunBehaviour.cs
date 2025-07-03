using System;
using System.Collections;
using System.Collections.Generic;
using Fusion.Addons.FSM;
using Fusion.Addons.KCC;

using UnityEngine;

public class RunBehaviour : AdvancedStateBehaviour
{
    [SerializeField]
    private DoingState _doingState;

    private StateMachine<State> _jumpMachine;


    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _doingState = new DoingState(customMachine.Kcc);
        _jumpMachine = new StateMachine<State>("Jump Machine",  _doingState);
        stateMachines.Add(_jumpMachine);
    }

    protected override void OnEnterState()
    {
        // Reset to Prepare state
        _jumpMachine.ForceActivateState(_doingState, true);

        //_characterController.Move(Vector3.up * 500);
    }
    protected override void OnEnterStateRender()
    {
        customMachine.Animancer.SetTrigger("Movement_normal");

    }


    protected override void OnFixedUpdate()
    {

       
    }

 

    [Serializable]
    public class DoingState : State
    {
        KCC _controller;
        public DoingState(KCC controller)
        {
            _controller = controller;
        }
        protected override void OnFixedUpdate()
        {
        }
        protected override void OnEnterState()
        {
        }
        protected override void OnEnterStateRender()
        {
            Debug.Log("Runing...");
        }
    }

}
