using UnityEngine;

namespace Flusk.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion Difference(this Quaternion lhs, Quaternion rhs)
        {
            Quaternion inverse = lhs * Quaternion.identity;
            return inverse * rhs;
        }
        
        //Gross
        public static void SetX(this Quaternion quaternion, float angle)
        {
            quaternion.eulerAngles.Set(angle, quaternion.eulerAngles.y, quaternion.eulerAngles.z);
        }
        
        public static void SetY(this Quaternion quaternion, float angle)
        {
            quaternion.eulerAngles.Set(quaternion.eulerAngles.x, angle, quaternion.eulerAngles.z);

        }
        
        public static void SetZ(this Quaternion quaternion, float angle)
        {
            quaternion.eulerAngles.Set(quaternion.eulerAngles.x, quaternion.eulerAngles.y, angle);

        }
    }
}