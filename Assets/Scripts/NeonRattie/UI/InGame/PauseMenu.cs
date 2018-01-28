using UnityEngine;

namespace NeonRattie.UI.InGame
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        protected KeyCode activateKeyCode;

        [SerializeField]
        private GameObject ui;

        public bool Active { get; private set; }

        public void Toggle()
        {
            Active = !Active;
            Time.timeScale = Active ? 0 : 1;
            ui.SetActive(Active);
        }
        
        public void Resume()
        {
            Active = false;
            Time.timeScale = Active ? 0 : 1;
            ui.SetActive(Active);
        }

        public void Pause()
        {
            Active = true;
            Time.timeScale = Active ? 0 : 1;
            ui.SetActive(Active);
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
                Toggle();
            }
        }
    }
}