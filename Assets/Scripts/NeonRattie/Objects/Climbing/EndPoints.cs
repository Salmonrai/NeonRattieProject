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
    }
}