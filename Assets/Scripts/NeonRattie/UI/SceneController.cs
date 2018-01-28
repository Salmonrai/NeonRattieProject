using System.Collections;
using Flusk.Patterns;
using Flusk.Utility;
using NeonRattie.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonRattie.UI
{
    public class SceneController : PersistentSingleton<SceneController>
    {
        [SerializeField] protected GameObject loading;
        
        public bool Loading { get; private set; }

        private Timer timer;

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

        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (SceneManager.GetActiveScene().name == "NewMain")
            {
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected virtual void Update()
        {
            if (timer != null)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene Loaded");
            if (scene.name == "NewMain")
            {
                Debug.Log("New main loaded");
                loading.gameObject.SetActive(true);
                timer = new Timer(8f, Deactivate);
            }
        }
        
        private IEnumerator LoadAsync(string name)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(name);
            loading.gameObject.SetActive(true);
            Loading = true;
            while (!async.isDone)
            {
                yield return null;
            }

            if (name != "NewMain")
            {
                loading.gameObject.SetActive(false);
            }
            Loading = false;
        }

        private void Deactivate()
        {
            loading.gameObject.SetActive(false);
            timer = null;
            SceneObjects.Instance.RatController.Init();
        }
    }
}
