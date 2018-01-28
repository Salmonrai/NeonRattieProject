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

        private IWalkable walkable;
        
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
            bool hasHit = Physics.SphereCast(ray: ray, radius: 0.5f, hitInfo: out hit,
                maxDistance: 1f, layerMask: rat.GroundLayer);
            if (!hasHit)
            {
                // Overlap sphere
                if (!FindPointWithSphere())
                {
                    return;
                }
            }

            walkable = hit.collider.GetComponent<IWalkable>();
            WalkingPoles pole = walkable as WalkingPoles;
            if ( pole != null )
            {
                point = hit.point;
                point.y = pole.GetY();
            }
            else
            {
                point = hit.point;
            }

            rat.SetWalkable(hit.collider.GetComponent<IWalkable>());
            Debug.Log(point);
            Debug.Log(hit.collider.gameObject);
            SetTweens(hit);
        }

        private bool FindPointWithSphere()
        {
            Collider[] colliders = Physics.OverlapSphere(rat.RatPosition.position, 1f, rat.GroundLayer);
            if (colliders.Length == 0)
            {
                //no walkables found
                rat.ChangeState(RatActionStates.Idle);
                return false;
            }

            Collider selected = colliders[0];
            IWalkable walkable = selected.GetComponent<IWalkable>();
            if (walkable == null)
            {
                return false;
            }
            Vector3 closestPoint = walkable.ClosestPoint(rat.RatPosition.position);
            WalkingPoles poles = walkable as WalkingPoles;
            if (poles != null)
            {
                closestPoint.y = poles.GetY();
                point = closestPoint;
            }
            else
            {
                point = closestPoint;
            }

            return true;
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
     
            Quaternion to = new Quaternion();
            
            WalkingPoles poles = walkable as WalkingPoles;
            if (poles != null)
            {
                to.SetLookRotation(poles.MoveDirection, poles.Up);
                Vector3 placePoint = poles.GetClosestStartPosition(rat.RatPosition.position).position + 
                                     Vector3.up * rat.IdealGroundDistance;
                position = new PositionTweener(rat.ClimbDownPolesCurve, rat.RatPosition.position,
                    placePoint, rat.RatPosition);
            }
            else
            {
                to.SetLookRotation(rat.RatPosition.up, hit.normal);
                position = new PositionTweener(rat.ClimbDownPolesCurve, rat.RatPosition.position,
                    hit.point + rat.RatCollider.bounds.extents * 0.5f, rat.RatPosition);
            }
            point = hit.point;
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

            if (walkable is WalkingPoles)
            {
                rat.ChangeState(RatActionStates.HorizontalPipeIdle);
                return;
            }
            rat.ChangeState(RatActionStates.Idle);
        }
    }
}