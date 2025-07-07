using System.Collections;
using System.Collections.Generic;
using Animancer;
using Fusion;
using Fusion.Addons.FSM;
using UnityEngine;
using Fusion.Addons.KCC;
public enum layerNames
{
    Base = 0,
    Upper = 1,
    Lower = 2,
    FullBody = 3
}

[RequireComponent(typeof(StateMachineController))]
public class PlayerAnimController : NetworkBehaviour, IStateMachineOwner, ICustomMachine
{

    [field: SerializeField] 
    public NetworkMecanimAnimator Animancer { get; private set; }

    [field: SerializeField] 
    public KCC Kcc { get; private set; }
   

    [SerializeField] private GameObject _movingBehaviours;
    private StateMachine<AdvancedStateBehaviour> _movingStateMachine;

    [SerializeField] private GameObject _stateBehaviours;
    private StateMachine<AdvancedStateBehaviour> _upperStateMachine;


   
    void IStateMachineOwner.CollectStateMachines(List<IStateMachine> stateMachines)
    {
        var moveBehaviours = _movingBehaviours.GetComponents<AdvancedStateBehaviour>();
     
        for (int i = 0; i < moveBehaviours.Length; ++i)
        {
            var state = moveBehaviours[i];
            state.customMachine = this;
        }
        _movingStateMachine = new StateMachine<AdvancedStateBehaviour>("Moving", moveBehaviours);
        stateMachines.Add(_movingStateMachine);

        var stateBehaviours = _stateBehaviours.GetComponents<AdvancedStateBehaviour>();        
        for (int i = 0; i < stateBehaviours.Length; ++i)
        {
            var state = stateBehaviours[i];
            state.customMachine = this ;
        }
        _upperStateMachine = new StateMachine<AdvancedStateBehaviour>("Behaviour", stateBehaviours);        
        stateMachines.Add(_upperStateMachine);
    }
    

    //[Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    //private void RPC_ChangeState<T>() where T : AdvancedStateBehaviour
    //{
    //    _upperStateMachine.TryActivateState<T>();
    //}
    bool isBattleMove;
    public override void FixedUpdateNetwork()
    {
    
        Animancer.Animator.SetLayerWeight((int)layerNames.Lower, 1 - PlayerInputContainer.Instance.Movement.magnitude * 0.6f);
        if (PlayerInputContainer.Instance .GetPressed(MyButtons.Jump))
        {
            if(_upperStateMachine.ActiveState is JumpBehaviour)
            {
                _movingStateMachine.TryActivateState<ClimbMoving>();
            }else
            {
                _upperStateMachine.TryActivateState<JumpBehaviour>();
            }

        }
        if(PlayerInputContainer.Instance.GetPressed(MyButtons.ChangeMoveMode))
        {
            isBattleMove = !isBattleMove;
            if (isBattleMove)
                _movingStateMachine.TryActivateState<BattleMoving>();
            else
                _movingStateMachine.TryActivateState<NormalMoving>();
        }

        if (PlayerInputContainer.Instance.GetPress(MyButtons.Attack))
        {
            _upperStateMachine.TryActivateState<AttackBehaviour>();
        }
        
    }
}