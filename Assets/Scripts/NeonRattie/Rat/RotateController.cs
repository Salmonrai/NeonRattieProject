using Flusk.Extensions;
using UnityEngine;

namespace NeonRattie.Rat
{
    public class RotateController : MonoBehaviour
    {

        private Quaternion goal;
        private float slerpTime;
        private float speed;


        public void SetLookDirection(Vector3 direction, Vector3 upAxis, float rotateSpeed = 1)
        {
            if (direction.sqrMagnitude <= float.Epsilon)
            {
                return;
            }
            //Debug.LogFormat("Direction: {0} - upAxis: {1}", direction, upAxis);
            goal = Quaternion.LookRotation(direction, upAxis);
            speed = rotateSpeed;
        }

        protected virtual void Update()
        {
            Quaternion current = transform.rotation;
            Quaternion next = Quaternion.Slerp(current, goal, slerpTime);
            transform.rotation = next;
            slerpTime += Time.deltaTime * speed;
            next.Difference(goal);
            float change = next.eulerAngles.y - goal.eulerAngles.y;
            if (Mathf.Abs(change) < 0.01f)
            {
                slerpTime = 0;
            }
        }
    }
}