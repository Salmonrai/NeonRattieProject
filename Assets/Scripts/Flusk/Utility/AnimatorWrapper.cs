using UnityEngine;

namespace Flusk.Utility
{
    public class AnimatorWrapper<T> where T : MonoBehaviour
    {
        protected Animator animator;
        
        public AnimatorWrapper(T component)
        {
            animator = component.GetComponent<Animator>();
            if (animator == null)
            {
                animator = component.GetComponentInChildren<Animator>();
            }
        }

        protected void SetBool(string property, bool value)
        {
            animator.SetBool(property, value);
        }

        protected bool GetBool(string property)
        {
            return animator.GetBool(property);
        }
    }
}