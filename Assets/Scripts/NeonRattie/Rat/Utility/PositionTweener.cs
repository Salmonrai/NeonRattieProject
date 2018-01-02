using UnityEngine;

namespace NeonRattie.Rat.Utility
{
    public class PositionTweener : AnimatorTweener<Vector3>
    {
        public PositionTweener(AnimationCurve curve, Vector3 initial, Vector3 final, Transform mover) : base(curve, initial, final, mover)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (CheckComplete())
            {
                IsComplete = true;
                return;
            }
            float value = animationCurve.Evaluate(currentTime);
            Vector3 point = Vector3.Slerp(from, to, value);   
            Set(point);
            currentTime += deltaTime * MultiplierModifier;
        }

        public override void Set(Vector3 attribute)
        {
            mover.position = attribute;
        }
    }
}