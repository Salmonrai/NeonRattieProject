using UnityEngine;

namespace NeonRattie.Objects
{
    public class WalkingPoles : WalkingPlane
    {
        [SerializeField]
        protected Transform walkPlane;
        public Transform WalkPlane
        {
            get { return walkPlane; }
        }

        [SerializeField]
        protected Transform endPoint;
        public Transform EndPoint
        {
            get { return endPoint; }
        }

        public Vector3 MoveDirection
        {
            get;
            private set;
        }

        public override Vector3 Up
        {
            get { return walkPlane.up; }
        }

        public float GetY()
        {
            return walkPlane.transform.position.y;
        }

        protected override void Awake()
        {
            base.Awake();
            MoveDirection = (endPoint.position - walkPlane.position).normalized;
        }
    }
}