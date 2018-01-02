using System;
using UnityEngine;

namespace NeonRattie.Rat.Utility
{
    public class LocalRotationTweener : RotationTweener
    {
        public LocalRotationTweener(AnimationCurve curve, Quaternion initial, Quaternion final, Transform mover) : base(curve, initial, final, mover)
        {
        }

        public override void Set(Quaternion attribute)
        {
            mover.localRotation = attribute;
        }
    }
}