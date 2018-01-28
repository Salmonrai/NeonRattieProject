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
            if (IsLoading())
            {
                return;
            }
            Active = !Active;
            Time.timeScale = Active ? 0 : 1;
            ui.SetActive(Active);
        }
        
        public void Resume()
        {
            if (IsLoading())
            {
                return;
            }
            Active = false;
            Time.timeScale = Active ? 0 : 1;
            ui.SetActive(Active);
        }

        public void Pause()
        {
            if (IsLoading())
            {
                return;
            }
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

        private bool IsLoading()
        {
            SceneController sc;
            if (!SceneController.TryGetInstance(out sc))
            {
                return false;
            }
            return sc.Loading;
        }
    }
}