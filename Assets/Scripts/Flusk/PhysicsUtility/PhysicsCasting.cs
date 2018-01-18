using System.Collections.Generic;
using UnityEngine;

namespace Flusk.PhysicsUtility
{
    public static class PhysicsCasting
    {
        public static RaycastHit[] RaycastForTypes<T>(Ray ray, float maxDistance, LayerMask mask) 
        {
            RaycastHit [] hits = Physics.RaycastAll(ray, maxDistance, mask);
            List<RaycastHit> validHits = new List<RaycastHit>();
            int length = hits.Length;
            for (int i = 0; i < length; i++)
            {
                RaycastHit current = hits[i];
                T component = current.collider.GetComponent<T>();
                if (component != null)
                {
                    validHits.Add(current);
                }
            }
            return validHits.ToArray();
        }

        public static bool RaycastForType<T>(Ray ray, out RaycastHit hit, float maxDistance, LayerMask mask) 
        {
            if (Physics.Raycast(ray, out hit, maxDistance, mask))
            {
                return hit.collider.GetComponent<T>() != null;
            }
            return false;
        }

        public static bool SphereCastForType<T>(Vector3 center, float radius, out RaycastHit info,
            Vector3 direction, float distance, LayerMask mask)
        {
            if (Physics.SphereCast(center, radius, direction, out info, distance, mask))
            {
                return info.collider.GetComponent<T>() != null;
            }

            return false;
        }

        public static T[] OverlapSphereForType<T>(Vector3 center, float radius, LayerMask mask)
        {
            Collider[] colliders = Physics.OverlapSphere(position: center, radius: radius, layerMask: mask);
            List<T> typeColliders = new List<T>();
            foreach (Collider collider in colliders)
            {
                T component = collider.GetComponent<T>();
                if (component != null)
                {
                    typeColliders.Add(component);
                }
            }
            return typeColliders.ToArray();
        }
    }
}