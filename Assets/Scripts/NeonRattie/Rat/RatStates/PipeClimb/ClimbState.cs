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
    }
}