using System;
using NeonRattie.Tools;
using UnityEditor;
using UnityEngine;

namespace NeonRattie.Objects.Climbing.Editor
{
    [CustomEditor(typeof(SeparateColliders))]
    public class SeparateCollidersEditor : UnityEditor.Editor
    {
        private SeparateColliders separate;

        protected virtual void OnEnable()
        {
            separate = (SeparateColliders) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            bool clicked = GUILayout.Button("Separate Colliders");
            if (clicked)
            {
                separate.Separate();
            }
        }
    }
}