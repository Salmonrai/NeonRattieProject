using NeonRattie.Rat;
using UnityEngine;

namespace Flusk.Extensions
{
    public static class MathExtensions
    {
        public static float Squared(this float a)
        {
            return a * a;
        }

        public static float Magnitude(float a, float b)
        {
            a = a.Squared();
            b = b.Squared();
            return Mathf.Sqrt(a + b);
        }

        public static Vector3 RotateVector(this Vector3 direction, float radian, Vector3 axis)
        {
            return RotateVector(direction, radian, axis, Vector3.zero);
        }
        
        /// <summary>
        /// Rotates the walk direction by angle value, aroud axis
        /// </summary>
        public static Vector3 RotateVector(this Vector3 direction, float radian, Vector3 n, Vector3 m)
        {            
            // Calculate the axis of rotation
            // Let's assume, the axis value is not normalized, or origned (for generalisation)
            Vector3 abc = m - n;
            float length = abc.magnitude;
            float vLength = Magnitude(abc.y, abc.z);
            
            // Translate n to the origin
            Vector4 n4 = n.ToVector4();
            n4.w = 1;
            Matrix4x4 translation = VectorExtensions.ToOrigin(n4);
            
            // Rotate about X-axis
            Vector3 dividedByV = abc / vLength;
            Matrix4x4 xRotate = MatrixExtensions.CenterFour(dividedByV.z, dividedByV.y, -dividedByV.y, dividedByV.z);

            // Rotate about the y-axis
            Vector3 dividedByL = abc / length;
            float vOverL = vLength / length;
            Matrix4x4 yRotate = MatrixExtensions.ConstructWithFloats
            (vOverL, 0, dividedByL.x, 0,
                0, 1, 0, 0,
                -dividedByL.x, 0, vOverL, 0,
                0, 0, 0, 1);
            
            // Rotate about the z axis
            Matrix4x4 zRotate = Matrix4x4.identity;;
            zRotate.m00 = Mathf.Cos(radian);
            zRotate.m01 = -Mathf.Sin(radian);
            zRotate.m10 = Mathf.Sin(radian);
            zRotate.m11 = Mathf.Cos(radian);
            
            // Reverse yRotate
            Matrix4x4 transposeYRotate = yRotate.transpose;
            
            // Reverse xRotate
            Matrix4x4 transposeXRotate = xRotate.transpose;
            
            // Reverse the translation matrix
            Matrix4x4 transposeTranslation = translation.transpose;
            
            // Calculate Total Transformation
            Matrix4x4 transformation = transposeTranslation *
                                       transposeXRotate *
                                       transposeYRotate *
                                       zRotate *
                                       yRotate *
                                       zRotate *
                                       translation;
            Vector3 result = transformation * direction;
            return result;
        }
    }
}