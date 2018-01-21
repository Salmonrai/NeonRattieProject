using System;
using Flusk.Utility;
using NeonRattie.Objects;
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

        private Vector3 up;

        private Vector3 point;
        
        public override void Enter(IState state)
        {
            base.Enter(state);

            FindPoint();
            rat.AddDrawGizmos(OnGizmosDrawn);
            
            rat.RatAnimator.PlayJump();
        }

        private void FindPoint()
        {
            Vector3 direction = rat.RatPosition.forward;
            Ray ray = new Ray(rat.RatPosition.position, direction);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, rat.ClimbOffDistance, rat.GroundLayer))
            {
                return;
            }

            point = hit.point;
            rat.SetWalkable(hit.collider.GetComponent<IWalkable>());
            Debug.Log(point);
            Debug.Log(hit.collider.gameObject);
            SetTweens(hit);
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
            
            rat.RatAnimator.PlayJump(false);
        }

        private void SetTweens(RaycastHit hit)
        {
            position = new PositionTweener(rat.ClimbDownPolesCurve, rat.RatPosition.position,
                hit.point + rat.RatCollider.bounds.extents * 0.5f, rat.RatPosition);
            point = hit.point;
            Quaternion to = new Quaternion();
            to.SetLookRotation(rat.RatPosition.up, hit.normal);
            rotation = new RotationTweener(rat.ClimbRotationCurve, rat.RatPosition.rotation, to, rat.RatPosition);
            rotation.MultiplierModifier = position.MultiplierModifier = 5f;
            rotation.Complete = position.Complete = OnComplete;
        }

        protected void OnGizmosDrawn()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point, 0.01f);
        }

        private void OnComplete()
        {
            if (!position.IsComplete || !rotation.IsComplete)
            {
                return;
            }
            rat.ChangeState(RatActionStates.Idle);
        }
    }
}