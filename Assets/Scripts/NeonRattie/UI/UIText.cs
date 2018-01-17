using UnityEngine;
using UnityEngine.UI;

namespace NeonRattie.UI
{
    [RequireComponent(typeof(Text))]
    public class UIText : RatUIComponent
    {
        private Text text;
        
        protected virtual void Awake()
        {
            text = GetComponent<Text>();
        }
    }
}