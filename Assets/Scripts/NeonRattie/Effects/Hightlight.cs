using UnityEngine;

namespace NeonRattie.Effects
{
    public class Hightlight : MonoBehaviour
    {
        [SerializeField]
        protected Material defaultMaterial;

        [SerializeField]
        protected Material hightlightMaterial;

        private new MeshRenderer renderer;

        public bool Highlighted { get; private set; }
        
        /// <summary>
        /// Activates the highlight based on the state
        /// </summary>
        public void Highlight(bool state = true)
        {
            Highlighted = state;
            if (hightlightMaterial == null || defaultMaterial == null)
            {
                return;
            }
            renderer.material = Highlighted ? hightlightMaterial : defaultMaterial;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
        }
    }
}