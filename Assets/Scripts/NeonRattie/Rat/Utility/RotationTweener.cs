using UnityEngine;

namespace NeonRattie.Rat.Utility
{
    public class RotationTweener : AnimatorTweener<Quaternion>
    {
        public RotationTweener(AnimationCurve curve, Quaternion initial, Quaternion final, Transform mover) : base(curve, initial, final, mover)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (CheckComplete())
            {
                IsComplete = true;
                Set(to);
                return;
            }
            float value = animationCurve.Evaluate(currentTime);
            Quaternion rotation = Quaternion.Slerp(from, to, value);
            Set(rotation);
            currentTime += deltaTime * MultiplierModifier;
        }

        public override void Set(Quaternion attribute)
        {
            mover.rotation = attribute;
        }
    }
}