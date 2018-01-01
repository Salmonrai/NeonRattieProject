using System;
using Flusk.Utility;
using NeonRattie.Rat.Utility;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbDown : RatState, IActionState, IClimb
    {
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbDown;}
        }

        private PositionTweener position;
        private RotationTweener rotation;

        private Vector3 point;
        
        public override void Enter(IState state)
        {
            base.Enter(state);

            FindPoint();

            rat.AddDrawGizmos(OnGizmosDrawn);
        }

        private void FindPoint()
        {
            Vector3 direction = (rat.RatPosition.forward + rat.RatPosition.up).normalized;
            Ray ray = new Ray(rat.RatPosition.position, direction);
            RaycastHit hit;
            Collider[] hits = Physics.OverlapSphere(rat.RatPosition.position, 1, rat.GroundLayer);
            if (Physics.Raycast(ray, out hit, rat.ClimbOffDistance, rat.GroundLayer))
            {
                point = hit.point;
                Debug.Log(point);
                SetTweens(hit);
            }
            else
            {
                rat.ChangeState(RatActionStates.ClimbIdle);
            }
        }

        public override void Tick()
        {
            position.Tick(Time.deltaTime);
            rotation.Tick(Time.deltaTime);
        }

        public override void Exit(IState state)
        {
            position = null;
            rotation = null;
            rat.RemoveDrawGizmos(OnGizmosDrawn);
        }

        private void SetTweens(RaycastHit hit)
        {
            position = new PositionTweener(rat.ClimbDownPolesCurve, rat.RatPosition.position,
                hit.point + rat.RatCollider.bounds.extents, rat.RatPosition);
            point = hit.point;
            Quaternion rot = new Quaternion {eulerAngles = Vector3.right * 90};
            Quaternion to = rat.RatPosition.rotation * rot;
            rotation = new RotationTweener(rat.ClimbRotationCurve, rat.RatPosition.rotation, to, rat.RatPosition);
        }

        protected void OnGizmosDrawn()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point, 0.01f);
        }
    }
}