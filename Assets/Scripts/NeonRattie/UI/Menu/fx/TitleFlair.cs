using UnityEngine;

namespace NeonRattie.UI.Menu.fx
{
    public class TitleFlair : MonoBehaviour
    {
        [SerializeField] protected float speed;

        protected virtual void Update()
        {
            float angle = Time.time * speed;
            Vector3 axis = transform.up;
            Quaternion rotation = transform.rotation;
            rotation.ToAngleAxis(out angle, out axis);
            transform.rotation = rotation;
        }
    }
}
