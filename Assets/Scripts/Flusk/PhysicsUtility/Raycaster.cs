using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flusk.PhysicsUtility
{
    [Serializable]
    public class Raycaster
    {
        /// <summary>
        /// The objects used to orientate the rays
        /// </summary>
        [SerializeField]
        protected Transform [] rayOrientation;

        /// <summary>
        /// Configures if the raycast calculation is constantly happening
        /// </summary>
        [SerializeField]
        protected bool isConstant;

        [SerializeField]
        protected float rayLength;

        [SerializeField]
        protected LayerMask mask;

        public Raycaster(Transform[] orientations, LayerMask layermask, float distance = float.MaxValue,
            bool constant = false)
        {
            rayOrientation = orientations;
            isConstant = constant;
            mask = layermask;
        }
        
        /// <summary>
        /// Returns a list of validly hit Raycast hits
        /// </summary>
        public RaycastHitBool[] Raycast(float maxDistance, LayerMask mask)
        {
            List<RaycastHitBool> results = new List<RaycastHitBool>();
            foreach (Transform transform in rayOrientation)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                bool isHit = Physics.Raycast(ray, out hit, maxDistance, mask);
                RaycastHitBool hitBool = new RaycastHitBool(hit, isHit);
                results.Add(hitBool);
            }
            return results.ToArray();
        }

        public RaycastHitBool[] Raycast()
        {
            return Raycast(rayLength, mask);
        }
        
        public RaycastHit[] TrimmedRaycast(float maxDistance, LayerMask mask)
        {
            RaycastHitBool[] results = Raycast(maxDistance, mask);
            List<RaycastHit> output = new List<RaycastHit>();
            foreach (RaycastHitBool result in results)
            {
                if (result.IsValid)
                {
                    output.Add(result.Hit);
                }
            }

            return output.ToArray();
        }

        public void Draw()
        {
            foreach (Transform transform in rayOrientation)
            {
                Gizmos.DrawRay(transform.position, transform.forward);
            }
        }
    }
}