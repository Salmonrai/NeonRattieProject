using System;
using Flusk.Extensions;
using Flusk.Utility;
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

        public enum Orientation
        {
            /// <summary>
            /// At the beginng of the transition, on a flat-ish surface
            /// </summary>
            Flat,
            
            /// <summary>
            /// The process of tweeing to the correct orientation
            /// </summary>
            Tweening,
            
            /// <summary>
            /// Ready to move upwards
            /// </summary>
            StraightUp
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
            if (rat.ClimbPole == null)
            {
                rat.ChangeState(((RatState)state).State);
                return;
            }

            Vector3 offset = rat.ClimbPole.Bounds.extents.normalized;
            offset = offset.Flatten();
            offset *= 0.02f;
            
            positionTweener =
                new PositionTweener(rat.ClimbUpPolesCurve, rat.RatPosition.position, rat.ClimbPole.Point + offset, rat.transform);
            rotationTweener = 
                new RotationTweener(rat.ClimbRotationCurve, rat.RatPosition.rotation, rat.ClimbPole.Rotation, rat.transform);

            positionTweener.MultiplierModifier = rotationTweener.MultiplierModifier = rat.ClimbMotionMultiplier;
            
            positionTweener.Complete += OnComplete;
            rotationTweener.Complete += OnComplete;

            rat.AttachedMonoBehaviours[typeof(RotateController)].enabled = false;
        }

        public override void Tick()
        {
            base.Tick();
            positionTweener.Tick(Time.deltaTime);
            rotationTweener.Tick(Time.deltaTime);
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
            rat.ChangeState(RatActionStates.ClimbIdle);
            positionTweener = null;
            rotationTweener = null;
        }
    }
}