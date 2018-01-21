using Flusk.Extensions;
using NeonRattie.Controls;
using NeonRattie.Objects;
using NeonRattie.UI;
using UnityEngine;

namespace NeonRattie.Rat.RatStates.PipeClimb
{
    public class ClimbState : RatState, IActionState
    {
        public override RatActionStates State
        {
            get { return RatActionStates.ClimbIdle; }
        }
        
        protected bool PolePoint (out Vector3 point)
        {
            Collider collider = rat.ClimbPole.GetComponent<Collider>();
            point = collider.bounds.ClosestPoint(rat.RatPosition.position);
            return true;
        }

        protected bool RotateToClimbPole(out RaycastHit hit, Vector3 up, Vector3 forward, float sign = 1)
        {
            ClimbPole pole = (ClimbPole) rat.CurrentClimbable;
            Vector3 toClimbPole = pole.Position - rat.RatPosition.position;
            toClimbPole = toClimbPole.Flatten().normalized;
            Ray ray = new Ray(rat.RatPosition.position, toClimbPole);
            if (!pole.Collider.Raycast(ray, out hit, float.MaxValue))
            {
                return false;
            }

            Vector3 normal = hit.normal;
            Vector3 tangent = normal.CalculateTangent(up, forward);

            Quaternion rotation = rat.RatPosition.rotation;
            rotation.SetLookRotation(tangent * sign, normal);
            rat.SetTransform(rat.RatPosition.position, rotation, rat.RatPosition.localScale);
            return true;
        }
        

        protected bool RotateToClimbPole(out RaycastHit hit, float sign = 1)
        {
            return RotateToClimbPole(out hit, Vector3.up, Vector3.forward, sign);
        }

        protected bool PolePoint(out RaycastHit hit)
        {
            Collider collider = rat.ClimbPole.GetComponent<Collider>();
            Ray ray = new Ray(rat.RatPosition.position + rat.RatPosition.up * 0.1f, -rat.RatPosition.up);
            return collider.Raycast(ray, out hit, float.MaxValue);
        }

        protected bool CalculateVectors(out Vector3 normal, out Vector3 tangent)
        {
            RaycastHit hit;
            if (PolePoint(out hit))
            {
                // The new up value
                normal = hit.normal;
                
                // The new forward value
                tangent = normal.CalculateTangent();
                return true;
            }
            normal = tangent = default(Vector3);
            return false;
        }

        protected virtual void OnGizmosDrawn()
        {
            Gizmos.color = Color.black;
            ClimbPole pole = rat.CurrentClimbable as ClimbPole;;
            Vector3 toClimbPole = pole.Position - rat.RatPosition.position;
            toClimbPole = toClimbPole.Flatten().normalized;
            Ray ray = new Ray(rat.RatPosition.position, toClimbPole);
            Gizmos.DrawRay(ray.origin, ray.direction);
        }
    }
}