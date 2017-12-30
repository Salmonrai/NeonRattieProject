using System;
using UnityEngine;

namespace NeonRattie.Rat.Utility
{
    public abstract class AnimatorTweener<TAttribute>
    {
        /// <summary>
        /// Fires at different points of the tween
        /// </summary>
        public Action Start, Update, Complete;
        
        /// <summary>
        /// The curve to move along
        /// </summary>
        protected AnimationCurve animationCurve;
        
        /// <summary>
        /// The initial point and the final point
        /// </summary>
        protected TAttribute to, from;

        /// <summary>
        /// The object to move
        /// </summary>
        protected Transform mover;

        /// <summary>
        /// Keeps track of the current animation curve state
        /// </summary>
        protected float currentTime;

        /// <summary>
        /// The length of the animation curve
        /// </summary>
        protected float finalTime;

        public float MultiplierModifier { get; set; }
        
        public bool IsComplete { get; protected set; }

        protected AnimatorTweener(AnimationCurve curve, TAttribute initial, TAttribute final, Transform mover)
        {
            animationCurve = curve;
            from = initial;
            to = final;
            this.mover = mover;
            currentTime = 0;

            if (animationCurve != null && animationCurve.length > 0)
            {
                finalTime = animationCurve[animationCurve.length - 1].time;
            }
        }

        public void SetCallbacks(Action start, Action update, Action complete)
        {
            Start = start;
            Update = update;
            Complete = complete;
        }

        protected bool CheckComplete()
        {
            IsComplete = currentTime >= finalTime;
            if (!IsComplete)
            {
                return IsComplete;
            }
            if (Complete != null)
            {
                Complete();
            }
            return IsComplete;
        }

        public abstract void Tick(float deltaTime);

        public abstract void Set(TAttribute attribute);
    }
}