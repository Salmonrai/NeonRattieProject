using UnityEngine;

namespace Flusk.Extensions
{
    public class MatrixExtensions
    {
        /// <summary>
        /// a b are middle second column
        /// c b are middle third column
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public static Matrix4x4 CenterFour(float a, float b, float c, float d)
        {
            Vector4 first = VectorExtensions.Right();
            Vector4 second = new Vector4(0, a, b, 0);
            Vector4 third = new Vector4(0, c, d, 0);
            Vector4 fourth = VectorExtensions.W();
            return new Matrix4x4(first, second, third, fourth);
        }

        /// <summary>
        /// by column
        /// </summary>
        public static Matrix4x4 ConstructWithFloats
            (float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            Vector4 first = new Vector4(m00, m01, m02, m03);
            Vector4 second = new Vector4(m10, m11, m12, m13);
            Vector4 third = new Vector4(m20, m21, m22, m23);
            Vector4 fourth = new Vector4(m30, m31, m32, m33);
            
            return new Matrix4x4(first, second, third, fourth);
        }
    }
}