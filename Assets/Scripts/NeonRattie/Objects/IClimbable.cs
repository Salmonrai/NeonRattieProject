using UnityEngine;

namespace NeonRattie.Objects
{
    public interface IClimbable
    {
        LayerMask Mask { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Bounds Bounds { get; }

        void Select(bool state);
    }
}