using Flusk.Utility;
using NeonRattie.Management;
using NeonRattie.Objects;
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

        /// <summary>
        /// The ray used to enter this state
        /// </summary>
        private Ray previousWalkRay;

        private Vector3 lookDirection;

        public override void Enter(IState state)
        {
            base.Enter(state);
            Orientate();
            
            rat.RatAnimator.PlayJump();
        }

        private void Orientate()
        {
            previousWalkRay = new Ray(rat.RatPosition.position, rat.PreviousWalkDirection);
            lookDirection = rat.AdjustToClimbable(rat.CurrentClimbable as ClimbPole, previousWalkRay);

            Vector3 offset = rat.RatPosition.up * 5;

            Vector3 position = (rat.CurrentClimbable as ClimbPole).ClosestPoint(rat.RatPosition.position) + offset;

            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(Vector3.up, rat.RatPosition.up);

            positionTweener =
                new PositionTweener(rat.ClimbUpPolesCurve, rat.RatPosition.position, position, rat.transform);
            rotationTweener =
                new RotationTweener(rat.ClimbRotationCurve, rat.RatPosition.rotation, rotation, rat.transform);

            positionTweener.MultiplierModifier = rotationTweener.MultiplierModifier = rat.ClimbMotionMultiplier;

            positionTweener.Complete += OnComplete;
            rotationTweener.Complete += OnComplete;
        }

        public override void Tick()
        {
            base.Tick();
            
            rat.RotateController.SetLookDirection(lookDirection, rat.RatPosition.up, 0.1f);
            positionTweener.Tick(Time.deltaTime);
            rotationTweener.Tick(Time.deltaTime);
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
            rat.RatAnimator.PlayJump(false);
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