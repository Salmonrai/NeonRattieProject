using UnityEngine;

namespace NeonRattie.Objects
{
    public interface IClimbable
    {
        LayerMask CollisionMask { get; }
        Vector3 Point { get; }
        Quaternion Rotation { get; }
        Bounds Bounds { get; }

        void Select(bool state);
    }
}