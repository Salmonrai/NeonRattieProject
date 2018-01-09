using UnityEngine;

namespace Flusk.PhysicsUtility
{
    public class RaycasterBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected Raycaster raycaster;

        [SerializeField]
        protected Transform[] raycastPoints;

        [SerializeField]
        protected LayerMask mask;

        [SerializeField]
        protected float maxDistance;
        
        public RaycastHit [] RaycastHitResults { get; protected set; }
        public RaycastHitBool [] RaycastHitBoolResults { get; protected set; }

        protected virtual void Awake()
        {
            raycaster = new Raycaster(raycastPoints, mask, maxDistance);
        }
        
        protected virtual void FixedUpdate()
        {
            RaycastHitBoolResults = raycaster.Raycast();
            RaycastHitResults = raycaster.TrimmedRaycast(maxDistance, mask);
        }

        private void OnDrawGizmosSelected()
        {
            if (raycaster != null)
            {
                raycaster.Draw();
            }
        }
    }
}