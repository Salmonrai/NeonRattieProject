using NeonRattie.Effects;
using NeonRattie.Objects.Climbing;
using UnityEngine;

namespace NeonRattie.Objects
{
    [RequireComponent(typeof(Rigidbody), typeof(Hightlight))]
    public class ClimbPole : MonoBehaviour, IClimbable
    {
        /// <summary>
        /// The mask the climb pole belongs to
        /// </summary>
        [SerializeField]
        protected LayerMask collisionMask;
        public LayerMask CollisionMask { get { return collisionMask; } }

        /// <summary>
        /// The point the that the rat uses to configure itself up
        /// </summary>
        [SerializeField]
        protected Transform climbPoint;
        public Vector3 Point { get { return climbPoint.position; } }
        public Quaternion Rotation {get { return climbPoint.rotation; }}
        public Bounds Bounds { get { return meshRenderer.bounds; } }

        [SerializeField]
        protected Transform[] climbOffPoints;
        
        public Collider Collider { get; private set; }
        
        private Hightlight hightlight;
        private MeshRenderer meshRenderer;

        private ClimbOffCollection collection;

        public void Select(bool state)
        {
            hightlight.Highlight(state);
        }

        public Transform GetClosestJumpOff(Vector3 point)
        {
            return collection.GetClosest(point);
        }

        protected virtual void Awake()
        {
            hightlight = GetComponentInChildren<Hightlight>();
            meshRenderer = climbPoint.GetComponent<MeshRenderer>();
            Collider = GetComponent<Collider>();
            
            collection = new ClimbOffCollection(climbOffPoints);
        }
    }
}