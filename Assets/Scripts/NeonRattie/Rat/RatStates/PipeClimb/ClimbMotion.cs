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

        //private Vector3 normal, tangent;
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

            Vector3 forward;
           
            // For controlling the rat motion forward
            Move(out forward);
            
            // Jump off check
            RaycastHit hitGround;
            Ray ray;
            bool groundIsClose;
            ChangeToClimbOff(out hitGround, out ray, out groundIsClose);
            if (groundIsClose)
            {
                return;
            }
            FallTowards(FallTowardsData, 1 << rat.ClimbPole.gameObject.layer, 0.1f);
        }

        private void ChangeToClimbOff(out RaycastHit hitGround, out Ray ray, out bool groundIsClose)
        {
            ray = new Ray(rat.RatPosition.position, rat.RatPosition.forward);
            groundIsClose = Physics.Raycast(ray, out hitGround, 0.05f, rat.GroundLayer);
            if (groundIsClose)
            {
                rat.ChangeState(RatActionStates.ClimbDown);
            }
        }

        private void Move(out Vector3 forward)
        {
            FallTowardsData = hit.point;
            forward = rat.RatPosition.position +
                      rat.RatPosition.forward * rat.RunSpeed * Time.deltaTime;
            if (Vector3.Distance(FallTowardsData, rat.RatPosition.position) < 0.4f)
            {
                rat.TryMove(forward);
                if (!PlayerControls.Instance.CheckKey(PlayerControls.Instance.ClimbUpKey))
                {
                    rat.ChangeState(RatActionStates.ClimbIdle);
                }
            }
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
            //Gizmos.DrawRay(point, normal);
            Gizmos.color = Color.magenta;
            //Gizmos.DrawRay(point,  tangent);
        }
    }
}