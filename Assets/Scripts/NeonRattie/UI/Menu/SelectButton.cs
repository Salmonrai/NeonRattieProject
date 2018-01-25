using NeonRattie.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace NeonRattie.UI.Menu
{
    public class SelectButton : MonoBehaviour
    {
        [SerializeField]
        protected MenuButtons [] buttons;

        [SerializeField]
        protected MenuButtons defaultButton;

        [SerializeField]
        protected ButtonSelectionRat selection;

        private MenuButtons Current
        {
            get { return buttons[currentIndex]; }
        }

        private PlayerControls playerControls;

        private int currentIndex = 0;

        protected virtual void Start()
        {
            if (!PlayerControls.TryGetInstance(out playerControls))
            {
                Debug.LogError("Player Controls not found");
            }
            UpdateIndex();
            selection.UpdateImage(Current.Button);
        }

        protected virtual void Update()
        {
            if (playerControls.CheckKeyDown(playerControls.Forward))
            {
                int next = currentIndex - 1;
                if (next < 0)
                {
                    next += buttons.Length;
                }
                currentIndex = next;
                selection.UpdateImage(Current.Button);
            }
            else if ( playerControls.CheckKeyDown(playerControls.Back))
            {
                currentIndex = (currentIndex + 1) % buttons.Length;
                selection.UpdateImage(Current.Button);
            }

            if (playerControls.CheckKeyDown(playerControls.Select))
            {
                Current.Invoke();
            }
        }

        private void UpdateIndex()
        {
            int length = buttons.Length;
            for (int i = 0; i < length; i++)
            {
                if (buttons[i] == defaultButton)
                {
                    currentIndex = i;
                }
            }
        }
    }
}
