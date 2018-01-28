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
        
        public Transform GetClosestStartPosition(Vector3 position)
        {
            float sqrPlane = Vector3.SqrMagnitude(position - walkPlane.position);
            float sqrEnd = Vector3.SqrMagnitude(position - endPoint.position);
            return (sqrEnd > sqrPlane) ? walkPlane : endPoint;
        }
        
        public Transform GetFurtherestStartPosition(Vector3 position)
        {
            float sqrPlane = Vector3.SqrMagnitude(position - walkPlane.position);
            float sqrEnd = Vector3.SqrMagnitude(position - endPoint.position);
            return (sqrEnd < sqrPlane) ? walkPlane : endPoint;
        }

        public void CalculateMoveDirection(Vector3 position)
        {
            Transform startPoint = GetClosestStartPosition(position);
            Transform endPoint = GetFurtherestStartPosition(position);
            MoveDirection = (endPoint.position - startPoint.position).normalized;
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