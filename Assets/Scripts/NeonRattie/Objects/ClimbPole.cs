using NeonRattie.Effects;
using UnityEngine;

namespace NeonRattie.Objects
{
    [RequireComponent(typeof(Rigidbody), typeof(Hightlight))]
    public class ClimbPole : MonoBehaviour, IClimbable
    {
        [SerializeField]
        protected LayerMask collisionMask;
        public LayerMask CollisionMask { get { return collisionMask; } }

        [SerializeField]
        protected Transform climbPoint;
        public Vector3 Point { get { return climbPoint.position; } }
        public Quaternion Rotation {get { return climbPoint.rotation; }}
        public Bounds Bounds { get { return meshRenderer.bounds; } }

        private Hightlight hightlight;
        private MeshRenderer meshRenderer;

        public void Select(bool state)
        {
            hightlight.Highlight(state);
        }

        protected virtual void Awake()
        {
            hightlight = GetComponentInChildren<Hightlight>();
            meshRenderer = climbPoint.GetComponent<MeshRenderer>();
            
        }
    }
}