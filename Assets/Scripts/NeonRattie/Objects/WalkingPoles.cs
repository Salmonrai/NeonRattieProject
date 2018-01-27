using UnityEngine;

namespace NeonRattie.Objects
{
    public class WalkingPoles : WalkingPlane
    {
        [SerializeField]
        protected Transform walkPlane;

        public Vector3 MoveDirection
        {
            get { return walkPlane.forward; }
        }

        public override Vector3 Up
        {
            get { return walkPlane.up; }
        }

        public float GetY()
        {
            return walkPlane.transform.position.y;
        }
    }
}