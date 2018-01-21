using NeonRattie.Management;
using UnityEngine;

namespace NeonRattie.Rat.Animation
{

    /// <summary>
    /// Class specifically for bridging to rat animator
    /// helps for update floats, string etc
    /// as well as playing animations through triggers, bools
    /// and directly through names
    /// </summary>
    public class RatAnimator : MonoBehaviour
    {      
        public RatAnimatorWrapper Wrapper { get; private set; }
        
        public void PlayIdle()
        {
            //Wrapper.Idle = true;
        }

        public void PlaySearchingIdle ()
        {
            //Wrapper.SearchIdle = true;
        }

        public void PlayWalk()
        {
            //Wrapper.Walk = true;
        }

        public void PlayJump()
        {
            //Wrapper.Jump = true;
        }

        public void PlayJumpOn ()
        {
            //Wrapper.JumpUp = true;
        } 

        public virtual void Start()
        {
            RatController rat = SceneObjects.Instance.RatController;
            if (rat == null)
            {
                rat = GetComponentInChildren<RatController>();
            }
            Wrapper = new RatAnimatorWrapper(rat);
        }
    }
}
