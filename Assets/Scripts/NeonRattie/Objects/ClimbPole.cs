using NeonRattie.Effects;
using NeonRattie.Objects.Climbing;
using UnityEngine;

namespace NeonRattie.Objects
{
    [RequireComponent(typeof(Rigidbody), typeof(Hightlight))]
    public class ClimbPole : MonoBehaviour, IClimbable, IWalkable
    {
        /// <summary>
        /// The mask the climb pole belongs to
        /// </summary>
        [SerializeField]
        protected LayerMask collisionMask;
        public LayerMask Mask { get { return collisionMask; } }

        /// <summary>
        /// The point the that the rat uses to configure itself up
        /// </summary>
        [SerializeField]
        protected Transform climbPoint;
        public Vector3 Position { get { return climbPoint.position; } }
        public Collider[] Colliders { get; private set; }
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

        public void CalculateFirstPosition(out Vector3 position, out Quaternion rotation)
        {
            position = Position;
            rotation = Rotation;
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
            Colliders = GetComponentsInChildren<Collider>();
            
            collection = new ClimbOffCollection(climbOffPoints);
        }
    }
}