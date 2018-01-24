using Flusk.Utility;
using NeonRattie.Controls;
using NeonRattie.Objects;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbMotion : ClimbState, IClimb
    {
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbMotion; }
        }
        private RaycastHit hit;
        private float sign = 1;

        private Vector3 FallTowardsData
        {
            get { return rat.PreviousClimbFallTowardsPoint; }
            set { rat.PreviousClimbFallTowardsPoint = value; }
        }
        
        public override void Enter(IState state)
        {
            base.Enter(state);
            PlayerControls.Instance.Unwalk += OnUnWalk;
            rat.AddDrawGizmos(OnGizmosDrawn);
            
            rat.RatAnimator.PlayScuttle();
        }
        public override void Tick()
        {
            base.Tick();
            if (!RotateToClimbPole(out hit, Vector3.up, Vector3.forward, sign))
            {
                return;
            }

            // For controlling the rat motion forward
            Vector3 forward;
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
            FallTowards(FallTowardsData, 1 << (rat.CurrentClimbable as ClimbPole).gameObject.layer, 0.1f);
        }

        public override void FixedTick()
        {
            TryJumpFromClimb();
        }

        private void ChangeToClimbOff(out RaycastHit hitGround, out Ray ray, out bool groundIsClose)
        {
            ray = new Ray(rat.RatPosition.position, rat.RatPosition.forward);
            groundIsClose = Physics.Raycast(ray, out hitGround, 5f, rat.GroundLayer);
            if (groundIsClose)
            {
                rat.ChangeState(RatActionStates.ClimbDown);
            }
        }

        private void Move(out Vector3 forward)
        {
            FallTowardsData = hit.point;
            PlayerControls pc;
            if (PlayerControls.TryGetInstance(out pc))
            {
                if (pc.CheckKey(pc.ClimDownKey))
                {
                    sign = 1;
                }
                else if (pc.CheckKey(pc.ClimbUpKey))
                {
                    sign = -1;
                }
            }
            
            forward = rat.RatPosition.position +
                      rat.RatPosition.forward * rat.RunSpeed * Time.deltaTime;
            
            
            bool sucesss = rat.TryMove(forward, rat.GroundLayer, 0.5f);
            if (!sucesss)
            {
                Debug.Log("Failed to move!");
            }
            if (!pc.CheckKey(pc.ClimbUpKey) && !pc.CheckKey(pc.ClimDownKey))
            {
                rat.ChangeState(RatActionStates.ClimbIdle);
            }
        }

        public override void Exit(IState nextState)
        {
            base.Exit(nextState);
            PlayerControls.Instance.Unwalk -= OnUnWalk;
            rat.RemoveDrawGizmos(OnGizmosDrawn);
            
            rat.RatAnimator.PlayScuttle(false);
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