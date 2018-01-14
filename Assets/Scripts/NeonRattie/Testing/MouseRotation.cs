using System;
using Flusk.Extensions;
using Flusk.Management;
using Flusk.Structures;
using NeonRattie.Management;
using NeonRattie.Rat;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private RatController rat;

        public Vector3 FlatForward()
        {
            return transform.forward;
        }

        public Vector3 FlatRight()
        {
            return transform.right;
        }
        
        private void Start()
        {
            rat = SceneObjects.Instance.RatController;
            fromRat = rat.RatPosition.position - transform.position;
        }

        protected virtual void Update()
        {
            AxisRotation();
            transform.position = rat.RatPosition.position - fromRat;
            Quaternion rot = transform.rotation;
            rot.SetLookRotation(transform.forward, rat.RatPosition.up);
            transform.rotation = rot;
            if (MustMaintainPlane)
            {
                transform.up = rat.RatPosition.up;
            }
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
                return;
            }
            var axis = Mathf.Abs(delta.y) < Mathf.Abs(delta.x) ? 
                new Vector3(0, Mathf.Clamp(delta.x, xRange.Min, xRange.Max)) : 
                new Vector3(Mathf.Clamp(-delta.y, yRange.Min, yRange.Max), 0);
            float angle = Mathf.Clamp(rotationSpeed * delta.magnitude, 0, maxAngleRotation);
            Quaternion deltaRotation = Quaternion.AngleAxis(angle, transform.up);
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