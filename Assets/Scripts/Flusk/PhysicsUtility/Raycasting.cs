using System.Collections.Generic;
using UnityEngine;

namespace Flusk.PhysicsUtility
{
    public static class Raycasting
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
    }
}