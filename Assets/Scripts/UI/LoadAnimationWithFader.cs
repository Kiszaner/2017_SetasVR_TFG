using UnityEngine;

namespace UBUSetasVR.UI
{
    public class LoadAnimationWithFader : AlphaFader
    {
        public float turnSpeed = 2.0f;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, turnSpeed) + transform.rotation.eulerAngles);
        }
    }
}
