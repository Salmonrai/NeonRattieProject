﻿using System;
using Flusk.PhysicsUtility;
using Flusk.Utility;
using NeonRattie.Objects;
using NeonRattie.Rat.Utility;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.HorizontalPipe
{
    public class PipeWalkToPlane : RatState
    {
        public override RatActionStates State
        {
            get { return RatActionStates.PipeToWalk; }
        }

        private Vector3 point;
        private float stateTime;
        private float speed = 5f;
        private Vector3 startPoint;
        
        public override void Enter(IState state)
        {
            base.Enter(state);
            
            // Decide where we are going
            point = rat.PipeToWalkable.ClosestPoint(rat.transform.position);
            stateTime = 0;
            rat.RatAnimator.PlayJump();
            startPoint = rat.RatPosition.position;
        }

        public override void Tick()
        {
            stateTime += Time.deltaTime * speed;
            Vector3 nextPoint = Vector3.Lerp(startPoint, point, stateTime);
            rat.SetTransform(nextPoint, rat.RatPosition.rotation, rat.RatPosition.localScale);

            if (stateTime >= 1)
            {
                rat.RatAnimator.PlayJump(false);
                rat.ChangeState(RatActionStates.Idle);
            }
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            rat.RatAnimator.PlayJump(false);
            rat.RatAnimator.PlayIdle();
        }
    }
}