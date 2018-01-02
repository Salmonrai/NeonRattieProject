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

        /// <summary>
        /// What the fuck is this?
        /// Try to calculate the tangent from a normal
        /// </summary>
        public static Vector3 CalculateTangent(this Vector3 normal)
        {
            Vector3 upTangent = Vector3.Cross(normal, Vector3.up).normalized;
            Vector3 forwardTangent = Vector3.Cross(normal, Vector3.forward).normalized;
            return upTangent.sqrMagnitude > forwardTangent.sqrMagnitude ? upTangent : forwardTangent;
        }
    }
}