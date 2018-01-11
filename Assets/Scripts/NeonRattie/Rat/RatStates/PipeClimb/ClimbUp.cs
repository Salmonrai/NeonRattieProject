﻿using Flusk.Utility;
using NeonRattie.Rat.Utility;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbUp : RatState, IActionState, IClimb
    {
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbUp; }
        }

        /// <summary>
        /// The Position tweener
        /// </summary>
        private PositionTweener positionTweener;

        /// <summary>
        /// The rotation tweener
        /// </summary>
        private RotationTweener rotationTweener;

        public override void Enter(IState state)
        {
            base.Enter(state);
            
            rat.AdjustToWalkable(rat.NextWalkable);
            
            if (rat.NextWalkable == null)
            {
                rat.ChangeState(((RatState)state).State);
                return;
            }

            Vector3 offset = rat.RatCollider.bounds.extents;
            offset *= 0.02f;

            Vector3 position = rat.NextWalkable.ClosestPoint(rat.RatPosition.position) + offset;
            
            positionTweener =
                new PositionTweener(rat.ClimbUpPolesCurve, rat.RatPosition.position, position, rat.transform);
            rotationTweener = 
                new RotationTweener(rat.ClimbRotationCurve, rat.RatPosition.rotation, rat.NextWalkable.Rotation, rat.transform);

            positionTweener.MultiplierModifier = rotationTweener.MultiplierModifier = rat.ClimbMotionMultiplier;
            
            
            
            positionTweener.Complete += OnComplete;
            rotationTweener.Complete += OnComplete;

            //rat.AttachedMonoBehaviours[typeof(RotateController)].enabled = false;
        }

        public override void Tick()
        {
            base.Tick();
            
            rat.AdjustToWalkable(rat.NextWalkable);

            positionTweener.Tick(Time.deltaTime);
            rotationTweener.Tick(Time.deltaTime);
        }

        public override void Exit(IState state)
        {
            //rat.AttachedMonoBehaviours[typeof(RotateController)].enabled = true;
        }

        private void OnComplete()
        {
            if (positionTweener == null || rotationTweener == null)
            {
                return;
            }
            if (!positionTweener.IsComplete || !rotationTweener.IsComplete)
            {
                return;
            }
            rat.ChangeState(RatActionStates.Idle);
            positionTweener = null;
            rotationTweener = null;
        }
    }
}