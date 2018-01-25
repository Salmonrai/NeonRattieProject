using UnityEngine;
using UnityEngine.UI;

namespace NeonRattie.UI
{
    [RequireComponent(typeof(Text))]
    public class UIText : RatUIComponent
    {
        public Text Text { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            Text = GetComponent<Text>();
        }
    }
}