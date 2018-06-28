using UnityEngine;

namespace UBUSetasVR
{
    /// <summary>
    /// Class that defines a method to draw concentric circles gizmos in the Unity editor.
    /// </summary>
    public class DrawCircleGizmo : MonoBehaviour
    {
        /// <summary>
        /// Inner circle radius.
        /// </summary>
        public float firstRadius = 1.3f;

        /// <summary>
        /// Outer circle radius.
        /// </summary>
        public float secondRadius = 1.7f;

        /// <summary>
        /// Unity method that draws Gizmo elements on the scene editor.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Transform t = GetComponent<Transform>();
            AuxiliarFunctions.DrawCircleGizmo(t, firstRadius, secondRadius);
        }
    }
}