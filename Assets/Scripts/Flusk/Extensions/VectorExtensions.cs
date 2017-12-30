using System;
using UnityEngine;

namespace Flusk.Extensions
{
    public enum VectorComponent
    {
        X,
        Y,
        Z
    }
    
    public static class VectorExtensions
    {
        /// <summary>
        /// Flatten along a particulat axis
        /// </summary>
        public static Vector3 Flatten(this Vector3 vector, VectorComponent component = VectorComponent.Y)
        {
            Vector3 flatten;
            float magnitude = vector.magnitude;
            Vector3 direction = vector.normalized;
            switch (component)
            {
                case VectorComponent.X:
                    direction.x = 0;
                    break;
                case VectorComponent.Y:
                    direction.y = 0;
                    break;
                case VectorComponent.Z:
                    direction.z = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("component", component, null);
            }
            flatten = direction * magnitude;
            return flatten;
        }
    }
}