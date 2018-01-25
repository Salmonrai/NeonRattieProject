using System.Collections;
using Flusk.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonRattie.UI
{
    public class SceneController : PersistentSingleton<SceneController>
    {
        [SerializeField] protected GameObject loading;

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void LoadSceneAsync(string name)
        {
            StartCoroutine(LoadAsync(name));
        }

        public void Quit()
        {
            Application.Quit();
        }

        private IEnumerator LoadAsync(string name)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(name);
            loading.gameObject.SetActive(true);
            while (!async.isDone)
            {
                yield return null;
            }
            loading.gameObject.SetActive(false);
        }
    }
}
