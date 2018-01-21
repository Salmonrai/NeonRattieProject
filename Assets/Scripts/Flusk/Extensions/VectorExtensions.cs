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
            return normal.CalculateTangent(Vector3.up, Vector3.forward);
        }

        public static Vector3 CalculateTangent(this Vector3 normal, Vector3 up, Vector3 forward)
        {
            Vector3 upTangent = Vector3.Cross(normal, up).normalized;
            Vector3 forwardTangent = Vector3.Cross(normal, forward).normalized;
            return upTangent.sqrMagnitude < forwardTangent.sqrMagnitude ? upTangent : forwardTangent;
        }

        public static Vector4 ToVector4(this Vector3 vector)
        {
            return (Vector4) vector;
        }
        
        public static Vector4 Right()
        {
            return Vector3.right.ToVector4();
        }
        
        public static Vector4 Up()
        {
            return Vector3.up.ToVector4();
        }
        
        public static Vector4 Forward()
        {
            return Vector3.forward.ToVector4();
        }
        
        public static Vector4 W()
        {
            Vector4 v = Vector4.zero;
            v.w = 1;
            return v;
        }

        public static Matrix4x4 ToOrigin(Vector4 vector)
        {
            Vector4 first = Right();
            Vector4 second = Up();
            Vector3 third = Forward();
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetColumn(0, first);
            matrix.SetColumn(1, second);
            matrix.SetColumn(2, third);
            matrix.SetColumn(3, vector);
            return matrix;
        }
    }
}