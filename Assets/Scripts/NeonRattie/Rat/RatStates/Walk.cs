using System;
using System.Security.Cryptography;
using Flusk.Extensions;
using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
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
            rat.RatAnimator.PlayScamper();
            PlayerControls.Instance.Unwalk += OnUnWalk;
            PlayerControls.Instance.Jump += OnJump;
        }

        public override void Tick()
        {
            base.Tick();
            if (Math.Abs(rat.WalkDirection.magnitude) < 0.001f)
            {
                rat.ChangeState(RatActionStates.Idle);
                return;
            }
            Adjust();
            ChangeStates();
        }


        private void Adjust()
        {
            Ray ray = new Ray(rat.ProjectedWalkPoint, rat.ProjectedInfo.normal);
            Vector3 point = ray.GetPoint(rat.IdealGroundDistance);
            point = (point - rat.RatPosition.position).normalized;
            rat.Walk(point);
            FallTowards();
            AdjustToPlane();
        }
        
        public override void FixedTick()
        {
            TryJumpFromClimb();
        }

        public override void Exit (IState nextState)
        {
            rat.RatAnimator.PlayScamper(false);
            PlayerControls.Instance.Unwalk -= OnUnWalk;
            PlayerControls.Instance.Jump -= OnJump;
        }
        
        private void ChangeStates()
        {
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

        private void OnUnWalk(float x)
        {
            StateMachine.ChangeState(RatActionStates.Idle);
        }

        protected override void OnGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(rat.NosePoint.position, noseHit.normal);
        }
    }
}
