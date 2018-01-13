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
            previousWalkRay = new Ray( rat.RatPosition.position, rat.PreviousWalkDirection);
            lookDirection = rat.AdjustToWalkable(rat.NextWalkable, previousWalkRay);
            if (rat.NextWalkable == null)
            {
                rat.ChangeState(((RatState)state).State);
                return;
            }

            Vector3 offset = rat.RatPosition.up * 5;

            Vector3 position = rat.NextWalkable.ClosestPoint(rat.RatPosition.position) + offset;
            
            positionTweener =
                new PositionTweener(rat.ClimbUpPolesCurve, rat.RatPosition.position, position, rat.transform);
            rotationTweener = 
                new RotationTweener(rat.ClimbRotationCurve, rat.RatPosition.rotation, rat.NextWalkable.Rotation, rat.transform);

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