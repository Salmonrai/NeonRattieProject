using System;
using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates
{
    public class Jump : RatState, IActionState
    {
        private float stateTime;

        private int maxFailedCollisions = 5;
        private int failedCollisions;

        private bool jumpForward;

        public override RatActionStates State 
        { 
            get {return RatActionStates.Jump;}
        }

        public override void Enter(IState previousState)
        {
            base.Enter(previousState);
            rat.RatAnimator.PlayJump();
            stateTime = 0;
            GetGroundData();
            rat.GetRatUI().JumpUI.Deactivate();
            failedCollisions = 0;
            jumpForward = PlayerControls.Instance.CheckKey(PlayerControls.Instance.Forward);
        }

        public override void Tick()
        {
            base.Tick();
            JumpCalculation();
            stateTime += Time.deltaTime * 2f;
            int length = rat.JumpArc.length;
            bool passed = rat.JumpArc[length - 1].time <= stateTime;
            if ( passed )
            {
                StateMachine.ChangeState(RatActionStates.Idle);
            }
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.RatAnimator.PlayJump(false);
        }

        private void JumpCalculation()
        {
            Vector3 forward;
            var up = NextPoint(stateTime, out forward);
            rat.TryMove(up + forward, rat.CollisionMask, 0.8f);
        }

        private Vector3 NextPoint(float time, out Vector3 forward)
        {
            float jumpMultiplier = rat.JumpArc.Evaluate(time);
            Vector3 force = (rat.JumpForce * -rat.Gravity.normalized * jumpMultiplier);
            Vector3 up = groundPosition + force;
            if (jumpForward)
            {
                forward = rat.RatPosition.forward * stateTime * rat.WalkSpeed;
            }
            else
            {
                forward = Vector3.zero;
            }
            return up;
        }
    }
}
