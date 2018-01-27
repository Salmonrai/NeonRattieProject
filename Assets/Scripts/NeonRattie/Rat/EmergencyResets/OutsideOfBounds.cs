using NeonRattie.Management;
using NeonRattie.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonRattie.Rat.EmergencyResets
{
    public class OutsideOfBounds : MonoBehaviour
    {
        [SerializeField]
        protected float outOfBoundsMagnitude = 320f;

        private float squareMagnitude;
        
        private Transform rat;
        
        private SceneController controller;

        protected virtual void Awake()
        {
            squareMagnitude = outOfBoundsMagnitude * outOfBoundsMagnitude;
        }

        protected virtual void Start()
        {
            controller = SceneController.Instance;
            rat = SceneObjects.Instance.RatController.RatPosition;
        }

        protected virtual void Update()
        {
            float sqr = rat.position.sqrMagnitude;
            if (sqr > squareMagnitude && !controller.Loading)
            {
                SceneController.Instance.LoadSceneAsync("Menu");
            }
        }
    }
}