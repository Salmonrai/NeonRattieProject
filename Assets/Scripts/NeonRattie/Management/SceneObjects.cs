using Flusk.Patterns;
using NeonRattie.Rat;
using NeonRattie.Testing;
using NeonRattie.UI;
using NeonRattie.Viewing;
using UnityEngine;

namespace NeonRattie.Management
{
    public class SceneObjects : Singleton<SceneObjects>
    {
        [SerializeField] 
        protected CameraControls cameraControls;
        public CameraControls CameraControls
        {
            get { return cameraControls; }
        }

        [SerializeField] 
        protected RatController ratController;
        public RatController RatController
        {
            get { return ratController; }
        }

        [SerializeField]
        protected MouseRotation mouseRotation;
        public MouseRotation MouseRotation
        {
            get { return mouseRotation; }
        }

        [SerializeField]
        protected RatUI ratUI;
        public RatUI RatUi
        {
            get { return ratUI; }
        }
    }
}