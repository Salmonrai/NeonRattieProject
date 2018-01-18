using UnityEngine;
using UnityEngine.UI;

namespace NeonRattie.UI
{
    [RequireComponent(typeof(Text))]
    public class UIText : RatUIComponent
    {
        private Text text;
        
        protected override void Awake()
        {
            base.Awake();
            text = GetComponent<Text>();
        }
    }
}