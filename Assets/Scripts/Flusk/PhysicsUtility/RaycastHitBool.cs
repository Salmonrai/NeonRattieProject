using UnityEngine;

namespace Flusk.PhysicsUtility
{
    public class RaycastHitBool
    {
        public RaycastHit Hit;
        public bool IsValid;

        public RaycastHitBool(RaycastHit hit, bool valid)
        {
            Hit = hit;
            IsValid = valid;
        }
    }
}