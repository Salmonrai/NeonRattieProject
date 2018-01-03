using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeonRattie.Objects.Climbing
{
    [Serializable]
    public class ClimbOffCollection : IList<Transform>
    {
        private readonly List<Transform> climbOffPoints;

        
        #region ctor
        public ClimbOffCollection()
        {
            climbOffPoints = new List<Transform>();
        }

        public ClimbOffCollection(Transform[] points)
        {
            climbOffPoints = new List<Transform>(points);
        }

        public ClimbOffCollection(List<Transform> points)
        {
            climbOffPoints = points;
        }
        #endregion
        
        public Transform GetClosest(Vector3 point)
        {
            Transform closest = default(Transform);
            float smallestMagnitude = float.MaxValue;
            foreach (Transform transform in climbOffPoints)
            {
                if (Vector3.Distance(transform.position, point) < smallestMagnitude)
                {
                    closest = transform;
                }
            }
            return closest;
        }
        
        #region IList implementation
        public IEnumerator<Transform> GetEnumerator()
        {
            return climbOffPoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) climbOffPoints).GetEnumerator();
        }

        public void Add(Transform item)
        {
            climbOffPoints.Add(item);
        }

        public void Clear()
        {
            climbOffPoints.Clear();
        }

        public bool Contains(Transform item)
        {
            return climbOffPoints.Contains(item);
        }

        public void CopyTo(Transform[] array, int arrayIndex)
        {
            climbOffPoints.CopyTo(array, arrayIndex);
        }

        public bool Remove(Transform item)
        {
            return climbOffPoints.Remove(item);
        }

        public int Count
        {
            get { return climbOffPoints.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<Transform>) climbOffPoints).IsReadOnly; }
        }

        public int IndexOf(Transform item)
        {
            return climbOffPoints.IndexOf(item);
        }

        public void Insert(int index, Transform item)
        {
            climbOffPoints.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            climbOffPoints.RemoveAt(index);
        }

        public Transform this[int index]
        {
            get { return climbOffPoints[index]; }
            set { climbOffPoints[index] = value; }
        }
        #endregion
    }
}