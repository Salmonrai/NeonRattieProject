using System;
using UnityEngine;

namespace NeonRattie.Objects.Climbing
{
    [Serializable]
    public struct EndPoints
    {
        public Transform Begin;
        public Transform End;

        public Vector3 DirectionFrom(Vector3 position)
        {
            float beginSize = Vector3.SqrMagnitude(position - Begin.position);
            float endSize = Vector3.SqrMagnitude(position - End.position);
            Vector3 closestPoint = (beginSize > endSize) ? End.position : Begin.position;
            Vector3 furthestPoint = (beginSize < endSize) ? End.position : Begin.position;
            return (furthestPoint - closestPoint).normalized;
        }

        public Transform Closest(Vector3 position)
        {
            float beginSize = Vector3.SqrMagnitude(position - Begin.position);
            float endSize = Vector3.SqrMagnitude(position - End.position);
            return (beginSize > endSize) ? End : Begin;
        }

        public Vector3 ClosestPoint(Vector3 position)
        {
            return Closest(position).position;
        }
    }
}