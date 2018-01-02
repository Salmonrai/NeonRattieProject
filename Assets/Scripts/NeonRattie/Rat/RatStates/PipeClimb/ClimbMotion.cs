using Flusk.Utility;
using NeonRattie.Controls;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbMotion : ClimbState, IClimb
    {
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbMotion; }
        }

        public override void Enter(IState state)
        {
            base.Enter(state);
            PlayerControls.Instance.Unwalk += OnUnWalk;
            
            rat.AddDrawGizmos(OnGizmosDrawn);
        }

        private Vector3 normal, tangent;
        private RaycastHit hit;

        private Vector3 FallTowardsData
        {
            get { return rat.PreviousClimbFallTowardsPoint; }
            set { rat.PreviousClimbFallTowardsPoint = value; }
        }
        
        public override void Tick()
        {
            base.Tick();
            if (!RotateToClimbPole(out hit))
            {
                return;
            }
            FallTowardsData = hit.point;
            Vector3 forward = rat.RatPosition.position +
                              rat.RatPosition.forward * rat.RunSpeed * Time.deltaTime;
            if (Vector3.Distance(FallTowardsData, rat.RatPosition.position) < 0.4f)
            {
                rat.TryMove(forward);
                if (!PlayerControls.Instance.CheckKey(PlayerControls.Instance.ClimbUpKey))
                {
                    rat.ChangeState(RatActionStates.ClimbIdle);
                }
            }

            FallTowards(FallTowardsData, 1 << rat.ClimbPole.gameObject.layer, 0.1f);
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
            PlayerControls.Instance.Unwalk -= OnUnWalk;
            rat.RemoveDrawGizmos(OnGizmosDrawn);
        }

        private void OnUnWalk(float amount)
        {
            rat.ChangeState(RatActionStates.ClimbIdle);
        }

        protected override void OnGizmosDrawn()
        {
            base.OnGizmosDrawn();
            Vector3 point = rat.RatPosition.position;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(point, normal);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(point,  tangent);
        }
    }
}