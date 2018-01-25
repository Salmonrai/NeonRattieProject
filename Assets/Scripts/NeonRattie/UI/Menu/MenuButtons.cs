using UnityEngine;
using UnityEngine.UI;

namespace NeonRattie.UI.Menu
{
    [RequireComponent(typeof(Button))]
    public class MenuButtons : MonoBehaviour
    {
        public Button Button { get; private set; }

        public void Invoke()
        {
            Button.onClick.Invoke();
        }

        protected virtual void Awake()
        {
            Button = GetComponent<Button>();
        }

    }
}
