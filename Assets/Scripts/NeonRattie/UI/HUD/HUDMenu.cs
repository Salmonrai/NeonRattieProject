using JetBrains.Annotations;
using NeonRattie.UI.InGame;
using UnityEngine;
using UnityEngine.UI;

namespace NeonRattie.UI.HUD
{
    public class HUDMenu : MonoBehaviour
    {
        [SerializeField]
        protected PauseMenu pauseMenu;

        [SerializeField]
        protected Image icon;

        [SerializeField]
        protected Sprite paused, played;
        
        [UsedImplicitly]
        public void TogglePause()
        {
            pauseMenu.Toggle();
            icon.sprite = pauseMenu.Active ? played : paused;
            icon.SetNativeSize();
        }

        protected virtual void Update()
        {
            icon.sprite = pauseMenu.Active ? played : paused;
            icon.SetNativeSize();
        }
    }
}