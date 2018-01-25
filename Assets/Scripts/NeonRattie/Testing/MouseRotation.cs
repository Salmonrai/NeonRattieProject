using System;
using Flusk.Management;
using Flusk.Structures;
using NeonRattie.Management;
using UnityEngine;

namespace NeonRattie.Testing
{
    public class MouseRotation : MonoBehaviour
    {
        [SerializeField]
        protected Range xRange;

        [SerializeField]
        protected Range yRange;

        [SerializeField]
        protected float rotationSpeed;

        [SerializeField]
        protected float maxAngleRotation;

        [SerializeField]
        protected float rotationSlerpSpeed;
        
        public bool MustMaintainPlane { get; set; }

        private Vector3 fromRat;

        private Quaternion originalRotation;


        public Vector3 FlatForward()
        {
            return transform.forward;
        }

        public Vector3 FlatRight()
        {
            return transform.right;
        }

        protected void Awake()
        {
            originalRotation = transform.rotation;
        }


        protected virtual void Update()
        {
            AxisRotation();
            Quaternion rot = transform.rotation;
            rot.SetLookRotation(transform.forward, transform.up);
            transform.rotation = rot;
        }
        
        private void AxisRotation()
        {
            MouseManager mm;
            if (!MouseManager.TryGetInstance(out mm))
            {
                return;
            }
            Vector3 delta = mm.ExpandedAxis;
            if (delta.magnitude <= float.Epsilon)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation,
                    Time.deltaTime * rotationSlerpSpeed);
                return;
            }
            var axis = Mathf.Abs(delta.y) < Mathf.Abs(delta.x) ? 
                new Vector3(0, Mathf.Clamp(delta.x, xRange.Min, xRange.Max)) : 
                new Vector3(Mathf.Clamp(-delta.y, yRange.Min, yRange.Max), 0);
            float angle = Mathf.Clamp(rotationSpeed * delta.magnitude, 0, maxAngleRotation);
            Quaternion deltaRotation = Quaternion.AngleAxis(angle, axis);
            if (Math.Abs(axis.y) < 0.001f)
            {
                Quaternion rot = transform.rotation;
                rot *= deltaRotation;
            }
            Quaternion next = transform.rotation * deltaRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, next, Time.deltaTime * rotationSlerpSpeed);
        }
    }
}