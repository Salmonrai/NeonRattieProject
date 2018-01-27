using UnityEngine;

namespace NeonRattie.Objects
{
    /// <summary>
    /// A simple implementation for all surfaces the rat can walk on
    /// </summary>
    public class WalkingPlane : MonoBehaviour, IWalkable
    {
        /// <summary>
        /// The orientation helper to provide insight into the orientation of the
        /// rat on this plane
        /// </summary>
        [SerializeField]
        protected Transform orientationHelper;
        
        /// <summary>
        /// The list of colliders attached
        /// </summary>
        [SerializeField]
        protected Collider[] colliders;
        public Collider Collider
        {
            get { return colliders[0]; }
        }
        public Collider[] Colliders
        {
            get { return colliders; }
        }
        
        /// <summary>
        /// Insight into the roation of the rat on this walkable
        /// </summary>
        public Quaternion Rotation
        {
            get { return orientationHelper.localRotation; }
        }

        /// <summary>
        /// Insight into the position of the rat on this plane
        /// </summary>
        public Vector3 Position
        {
            get { return orientationHelper.position; }
        }

        /// <summary>
        /// The mask labeled on this plane
        /// </summary>
        public LayerMask Mask
        {
            get { return gameObject.layer; }
        }

        /// <inheritdoc />
        /// <summary>
        /// The up orientation of this plane
        /// </summary>
        public virtual Vector3 Up
        {
            get { return orientationHelper.up; }
        }

        public void CalculateFirstPosition(out Vector3 position, out Quaternion rotation)
        {
            position = Position;
            rotation = Rotation;
        }

        /// <summary>
        /// finds closest point on all colliders
        /// </summary>
        public Vector3 ClosestPoint(Vector3 point)
        {
            Vector3 closestPoint = Vector3.negativeInfinity;
            float smallestDistance = float.MaxValue;
            foreach (Collider current in colliders)
            {
                Vector3 closest = current.ClosestPointOnBounds(point);
                float distance = Vector3.Distance(closest, point);
                if (Vector3.Distance(closest, point) < smallestDistance)
                {
                    closestPoint = closest;
                    smallestDistance = distance;
                }
            }
            return closestPoint;
        }

        /// <summary>
        /// cast against all the colliders
        /// </summary>
        public bool Raycast(Ray ray, out RaycastHit info, float maxDistance)
        {
            foreach (Collider current in colliders)
            {
                if( current.Raycast(ray, out info, maxDistance) )
                {
                    return true;
                }
            }
            info = default(RaycastHit);
            return false;
        }

        /// <summary>
        /// Cache
        /// </summary>
        protected virtual void Awake()
        {
            if (colliders.Length == 0)
            {
                colliders = GetComponentsInChildren<Collider>();
            }
            if (orientationHelper == null)
            {
                orientationHelper = transform;
            }
        }
    }
}