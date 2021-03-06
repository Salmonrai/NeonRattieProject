﻿using UnityEngine;

namespace NeonRattie.Objects
{
    public interface IWalkable
    {
        /// <summary>
        /// The layer mask the attached object resides on
        /// </summary>
        LayerMask Mask { get; }
        
        /// <summary>
        /// The main collider attached to the object
        /// </summary>
        Collider Collider { get; }
        
        /// <summary>
        /// The list of all attached colliders,
        /// often this will probably be a list of one
        /// </summary>
        Collider [] Colliders { get; }
        
        /// <summary>
        /// Some insight into the orientation the rat needs to be
        /// when on this surface
        /// </summary>
        Quaternion Rotation { get; }
        
        /// <summary>
        /// Some insight into the position of the rat when on this walkable object
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// The up orientation of this plane
        /// </summary>
        Vector3 Up { get; }

        /// <summary>
        /// Given the current rat position, calculates where
        /// the rat should land when coming onto this walkable
        /// </summary>
        void CalculateFirstPosition(out Vector3 position, out Quaternion rotation);

        /// <summary>
        /// Finds the closest point on all colliders
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        Vector3 ClosestPoint(Vector3 point);

        bool Raycast(Ray ray, out RaycastHit info, float maxDistance);
    }
}