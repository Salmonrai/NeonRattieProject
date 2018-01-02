using UnityEngine;

namespace NeonRattie.Rat.Utility
{
    public class LocalPositionTweener : PositionTweener
    {
        public LocalPositionTweener(AnimationCurve curve, Vector3 initial, Vector3 final, Transform mover) : base(curve, initial, final, mover)
        {
        }

        public override void Set(Vector3 attribute)
        {
            mover.localPosition = attribute;
        }
    }
}