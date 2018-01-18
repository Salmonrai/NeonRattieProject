using System;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Walk : RatState, IActionState
    {
        public override RatActionStates State 
        { 
            get {return RatActionStates.Walk;}
        }
        
        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayWalk();
            PlayerControls.Instance.Unwalk += OnUnWalk;
            PlayerControls.Instance.Jump += OnJump;
        }

        public override void Tick()
        {
            base.Tick();
            if (Math.Abs(rat.WalkDirection.magnitude) < 0.001f)
            {
                rat.ChangeState(RatActionStates.Idle);
            }
            rat.Walk(rat.WalkDirection);
            AdjustToPlane();
            FallTowards();
            if (rat.ClimbUpValid())
            {
                rat.ChangeState(RatActionStates.ClimbUp);
                return;
            }
            if (rat.JumpOnValid())
            {
                rat.ChangeState(RatActionStates.JumpOn);
                return;
            }
            if (rat.JumpOffValid())
            {
                rat.ChangeState(RatActionStates.JumpOff);
            }
            
        }

        public override void Exit (IState nextState)
        {
            PlayerControls.Instance.Unwalk -= OnUnWalk;
            PlayerControls.Instance.Jump -= OnJump;
        }

        private void OnUnWalk(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }
    }
}
