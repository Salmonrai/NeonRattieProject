using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NeonRattie.Objects.Climbing.Editor
{
    [CustomEditor(typeof(ClimbPole))]
    public class ClimbPoleEditor : UnityEditor.Editor
    {
        private const string CLIMB_OFF_POINTS_NAME = "climbOffPoints";
        private SerializedProperty climbOffPointsArray;

        private Transform[] climbOffPoints;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            NormalizeClimbOffPoints();
        }

        private void NormalizeClimbOffPoints()
        {
            if (!GUILayout.Button("Normalize Jump Off Points"))
            {
                return;
            }

            var length = climbOffPointsArray.arraySize;

            // Calculate data
            var center = default(Vector3);
            float distance = 0;

            // Cache the array of transforms
            var list = new List<Transform>();
            for (var i = 0; i < length; i++)
            {
                var property = climbOffPointsArray.GetArrayElementAtIndex(i);
                var current = (Transform) property.objectReferenceValue;
                list.Add(current);
                center += current.position;
            }

            center /= length;

            // Calculate the mean distance the center
            foreach (var transform in list)
            {
                distance += (center - transform.position).magnitude;
            }

            distance /= length;

            // Set the points to the correct position
            foreach (var transform in list)
            {
                var direction = (center - transform.position).normalized;
                transform.position = center + direction * distance;
                
                Quaternion quaternion = new Quaternion();
                quaternion.SetLookRotation(direction, transform.up);
                transform.rotation = quaternion;
            }
        }

        private void OnEnable()
        {
            climbOffPointsArray = serializedObject.FindProperty(CLIMB_OFF_POINTS_NAME);
        }
    }
}