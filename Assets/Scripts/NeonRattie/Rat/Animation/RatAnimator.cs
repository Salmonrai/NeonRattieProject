using System;
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

        //Until we need it
        /*
        public event Action IdleComplete;
        public event Action ScamperComplete;
        public event Action ScuttleComplete;
        public event Action JumpComplete;
        */
        public event Action LongIdleComplete;
        
        
        public void PlayIdle(bool state = true)
        {
            Wrapper.Idle = state;
        }

        public void PlayScamper(bool state = true)
        {
            Wrapper.Scamper = state;
        }

        public void PlayJump(bool state = true)
        {
            Wrapper.Jump = state;
        }

        public void PlayScuttle (bool state = true)
        {
            Wrapper.Scuttle = state;
        }

        public void PlayLongIdle(bool state = true, Action action = null)
        {
            Wrapper.LongIdle = state;
            LongIdleComplete = action;
        }

        public void LongIdleEnd()
        {
            if (LongIdleComplete != null)
            {
                LongIdleComplete();
            }
        }

        public virtual void Awake()
        {
            RatController rat = GetComponent<RatController>();
            if (rat == null)
            {
                rat = GetComponentInChildren<RatController>();
            }
            Wrapper = new RatAnimatorWrapper(rat);
        }
    }
}
