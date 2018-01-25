using UnityEngine;
using UnityEngine.UI;
using System;

namespace NeonRattie.UI.Menu
{
    public class ButtonSelectionRat : MonoBehaviour
    {
        [SerializeField]
        protected Selection[] selection;

        [SerializeField] protected int defaultIndex;

        private Image image;

        public void UpdateImage(Button graphic)
        {
            foreach (Selection select in selection)
            {
                if (select.Select != graphic)
                {
                    continue;
                }
                image.sprite = select.Presentation;
                break;
            }
        }

        protected virtual void Awake()
        {
            image = GetComponent<Image>();
            image.sprite = selection[defaultIndex].Presentation;
        }

    }

    [Serializable]
    public struct Selection
    {
        public Button Select;
        public Sprite Presentation;

    }
}
