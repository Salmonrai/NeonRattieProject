using UnityEngine;

namespace NeonRattie.UI.InGame
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        protected KeyCode activateKeyCode;

        [SerializeField]
        private GameObject ui;

        private bool active = false;

        public void Resume()
        {
            active = !active;
            Time.timeScale = active ? 0 : 1;
            ui.SetActive(active);
        }

        public void Exit()
        {
            SceneController.Instance.LoadSceneAsync("Menu");
            Time.timeScale = 1;
        }

        public void Credits()
        {
            SceneController.Instance.LoadSceneAsync("Credits");
            Time.timeScale = 1;
        }
        
        protected virtual void Update()
        {
            if (SceneController.Instance.Loading)
            {
                return;
            }
            
            if (Input.GetKeyDown(activateKeyCode))
            {
                active = !active;
                Time.timeScale = active ? 0 : 1;
            }
            ui.SetActive(active);
        }
    }
}