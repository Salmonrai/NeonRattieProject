using System;
using System.Collections.Generic;
using Flusk.Patterns;
using UnityEngine;
using UnityEngine.UI;

namespace NeonRattie.UI
{
    public class RatUI : Singleton<RatUI>
    {
        [SerializeField]
        protected RatUIComponent climbUI;
        public RatUIComponent ClimbUI
        {
            get { return climbUI; }
        }

        [SerializeField]
        protected RatUIComponent jumpUI;
        public RatUIComponent JumpUI
        {
            get { return jumpUI; }
        }
        
        public RatUIComponent CurrentComponent { get; private set; }

        public void DisplayClimb()
        {
            Set(climbUI);
        }

        public void DisplayJump()
        {
            Set(jumpUI);
        }

        private void Set(RatUIComponent component)
        {
            component.Activate();
            if (CurrentComponent != null)
            {
                CurrentComponent.Deactivate();
            }
            CurrentComponent = component;
        }
    }
}