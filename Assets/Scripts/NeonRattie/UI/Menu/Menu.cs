using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonRattie.UI.Menu
{
    public class Menu : MonoBehaviour
    {
        public void Play()
        {
            SceneController.Instance.LoadSceneAsync("NewMain");
        }

        public void Credits()
        {
            SceneController.Instance.LoadSceneAsync("Credits");
        }

        public void Quit()
        {
            SceneController.Instance.Quit();
        }
    }
}