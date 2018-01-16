using System;
using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbIdle : ClimbState, IClimb
    {
        
        
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbIdle; }
        }

        public override void Enter(IState state)
        {
            base.Enter(state);
            PlayerControls.Instance.ClimbUp += OnClimb;
            PlayerControls.Instance.ClimbDown += OnClimb;
            rat.AddDrawGizmos(OnGizmosDrawn);
        }

        public override void Tick()
        {
            RaycastHit hit;
            if (RotateToClimbPole(out hit))
            {
                rat.PreviousClimbFallTowardsPoint = hit.point;
                FallTowards(rat.PreviousClimbFallTowardsPoint, 1 << (rat.CurrentClimbable as ClimbPole).gameObject.layer, 0.1f);
            }
        }

        public override void Exit(IState state)
        {
            base.Exit(state);
            PlayerControls.Instance.ClimbUp -= OnClimb;
            PlayerControls.Instance.ClimbDown -= OnClimb;
            rat.RemoveDrawGizmos(OnGizmosDrawn);
        }

        private void OnClimb(float amount)
        {
            rat.ChangeState(RatActionStates.ClimbMotion);
        }

        private void PushBackFromCollider()
        {
            Vector3 down = -rat.RatPosition.up;
            ClimbPole climbPole = rat.ClimbPole;
            if (climbPole == null)
            {
                return;
            }
            Collider collider = climbPole.GetComponentInChildren<Collider>();

            Ray ray = new Ray(rat.RatPosition.position, down);
            RaycastHit info;
            if (collider.Raycast(ray, out info, float.MaxValue))
            {
                rat.FindLowestPoint();
                float magnitude = Vector3.Distance(rat.LowestPoint, rat.RatPosition.position);
                Vector3 point = info.point + info.normal * magnitude;
                rat.SetTransform(point, rat.ClimbPole.Rotation, rat.RatPosition.localScale);
            }
            else
            {
                Debug.Log("How are you even in this state?");
            }
        }
    }
}