using UnityEngine;

namespace NeonRattie.Tools
{
    public class SeparateColliders : MonoBehaviour
    {
        [SerializeField]
        protected bool destroyOriginalColliders;
        
        private BoxCollider[] colliders;

        private string CHILD_NAME = "New Colliders";
        
        #if UNITY_EDITOR
        public void Separate()
        {
            colliders = GetComponents<BoxCollider>();
            int length = colliders.Length;
            Transform previous = transform.Find(CHILD_NAME);
            if (previous != null)
            {
                DestroyImmediate(previous.gameObject);
            }
            GameObject newChild = new GameObject(CHILD_NAME);
            newChild.transform.parent = transform;
            newChild.transform.localPosition = Vector3.zero;
            for (int i = 0; i < length; i++)
            {
                GameObject current = new GameObject(i.ToString());
                current.transform.SetParent(newChild.transform);
                current.transform.localPosition = Vector3.zero;
                BoxCollider newCollider = current.AddComponent<BoxCollider>();
                BoxCollider currentCollider = colliders[i];
                newCollider.size = currentCollider.size;
                newCollider.transform.localPosition = currentCollider.center;
                if (destroyOriginalColliders)
                {
                    DestroyImmediate(currentCollider);
                }
            }
        }
        #endif
    }
}