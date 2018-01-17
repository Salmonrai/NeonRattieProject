using System;
using UnityEngine;

namespace NeonRattie.UI
{
    // ReSharper disable once InconsistentNaming
    public class RatUIComponent : MonoBehaviour
    {
        [SerializeField]
        protected bool beginActive;
        
        protected RatUI uiManager;
        
        public virtual void Activate()
        {
            beginActive = true;
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        protected virtual void Awake()
        {
            uiManager = GetComponentInParent<RatUI>();
        }

        protected virtual void Start()
        {
            if (!beginActive)
            {
                Deactivate();
            }
        }
    }
}