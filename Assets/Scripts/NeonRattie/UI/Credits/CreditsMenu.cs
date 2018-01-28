using UnityEngine;

namespace NeonRattie.UI.Credits
{
    public class CreditsMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneController.Instance.LoadSceneAsync("NewMain");
        }

        public void Menu()
        {
            SceneController.Instance.LoadSceneAsync("Menu");
        }
    }
}