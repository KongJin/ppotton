using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion.Addons.FSM;
using Fusion.Addons.KCC;
using UnityEngine;

public class ClimbMoving : AdvancedStateBehaviour
{

    Animator _anim;
    Transform _reporter;
    KCC _kcc;

    [Tooltip("광선을 쏠 최대 거리")]
    [SerializeField] float maxDistance = 1.5f;
    [Tooltip("충돌을 검사할 레이어")]
    [SerializeField] LayerMask collisionMask = 1;

    private StateMachine<AdvancedState> _climbMachine;

    Vector3 HeadPos = new Vector3(0, 1.8f, 0);
    bool IsCloseWall()=>
        Physics.Raycast(_kcc.transform.position+ HeadPos, _kcc. transform.forward, maxDistance, ~0, QueryTriggerInteraction.Ignore);

    protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
    {
        _reporter = Repoter.Instance.transform;
        _anim = customMachine.Animancer.Animator;
        _kcc = customMachine.Kcc;
        _climbMachine = new StateMachine<AdvancedState>("Climb Machine",
            new ClimbState(customMachine),
            new HangCrouchState(customMachine),
            new JumpFromWallState(customMachine));
        stateMachines.Add(_climbMachine);
    }

    protected override void OnEnterState()
    {
        AnimUtil.FadeLayerWeightAsync(_anim, layerNames.FullBody, 1f, 0.4f);
        _climbMachine.TryActivateState<ClimbState>();
    }

    protected override bool CanEnterState() => IsCloseWall();

    protected override bool CanExitState(AdvancedStateBehaviour nextState) => 
        _climbMachine.ActiveState.IsDone ;
    

    protected override void OnExitState()
    {
        AnimUtil.FadeLayerWeightAsync(_anim, layerNames.FullBody, 0f, 0.7f);

    }
    protected override void OnFixedUpdate()
    {
        
        if (CanExitState(null)) Machine.TryDeactivateState(StateId);
        if (IsCloseWall())
        {
            if (PlayerInputContainer.Instance.GetPressed(MyButtons.Jump))
            {
                _climbMachine.TryActivateState<JumpFromWallState>();
                return;
            }
        }
        else
        {
            _climbMachine.TryActivateState<HangCrouchState>();
        }
    }
    [Serializable]
    public class HangCrouchState : AdvancedState
    {
        KCC _kcc;
        Animator _anim;
        public HangCrouchState(ICustomMachine cMachine) : base(cMachine)
        {
            _kcc = cMachine.Kcc;
            _anim = cMachine.Animancer.Animator;
        }
        protected override void OnFixedUpdate()
        {
            IsDone = Machine.StateTime > 1.5f || _kcc.Data.IsGrounded; //Machine.StateTime > 1.13f;
            _kcc.SetDynamicVelocity(Vector3.zero);
            _kcc.SetKinematicVelocity((_kcc.transform.up + _kcc.transform.forward));
        }
        protected override bool CanEnterState() => Machine.ActiveState is ClimbState;
        protected override void OnEnterState()
        {
            _anim.SetBool("hangCrouch", true);
            //_kcc.SetDynamicVelocity((_kcc.transform.up + _kcc.transform.forward) * 10);
        }
        protected override void OnExitState()
        {
            base.OnExitState();
            _anim.SetBool("hangCrouch", false);
        }
    }

    [Serializable]
    public class ClimbState : AdvancedState
    {
        KCC _kcc;
        Transform _reporter;
        Animator _anim;
        Vector3 _movement;
        public ClimbState(ICustomMachine cMachine) : base(cMachine)
        {
            _kcc = cMachine.Kcc;
            _anim = cMachine.Animancer.Animator;
            _reporter = Repoter.Instance.transform;
        }
        protected override void OnFixedUpdate()
        {
            IsDone = _kcc.Data.IsGrounded;
            _kcc.SetDynamicVelocity(Vector3.zero);
            var movement = _reporter.InverseTransformDirection(PlayerInputContainer.Instance.Movement);
            _movement.y = movement.z;//(앞뒤) 를 (위아래)로 변환
            _anim.SetFloat("climbFloat", Mathf.InverseLerp(-1f, 1f, _movement.y));
            _kcc.SetKinematicVelocity(_movement);
        }

        protected override void OnEnterState()
        {
            _anim.SetBool("climbBool",true);
            //var proc = _kcc.GetComponent<EnvironmentProcessor>();            
            //proc.Gravity = new Vector3(0, 0.1f, 0);
            //_kcc.ExecuteStage<ISetGravity>();
        }

        protected override void OnExitState()
        {
            base.OnExitState();
            _anim.SetBool("climbBool", false);
        }
    }

    [Serializable]
    public class JumpFromWallState : AdvancedState
    {
        KCC _kcc;
        Animator _anim;
        public JumpFromWallState(ICustomMachine cMachine):base(cMachine)
        {
            _kcc = cMachine.Kcc;
            _anim = cMachine.Animancer.Animator;
        }

        protected override bool CanEnterState()=> Machine.ActiveState is ClimbState;
        
        protected override void OnFixedUpdate()
        {
            IsDone = _kcc.Data.IsGrounded;
            
            if (Machine.StateTime < 0.7f)
            {
                _kcc.SetKinematicVelocity(Vector3.zero);
                _kcc.SetDynamicVelocity(Vector3.zero);
            }else
            {
                _kcc.SetInputDirection( -_kcc.transform.forward*100);
            }
        }
        protected override void OnEnterState()
        {
            _anim.SetBool("jumpFromWall",true);
        }

        protected override void OnExitState()
        {
            base.OnExitState();
            _anim.SetBool("jumpFromWall", false);
        }
    }

   
}
