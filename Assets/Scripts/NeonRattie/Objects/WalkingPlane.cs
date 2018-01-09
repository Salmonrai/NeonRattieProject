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
            get { return orientationHelper.rotation; }
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
    }
}