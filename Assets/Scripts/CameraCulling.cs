using UnityEngine;

namespace UBUSetasVR
{
    /// <summary>
    /// Class that defines a camera culling method for items in the Environment layer mask.
    /// </summary>
    public class CameraCulling : MonoBehaviour
    {
        /// <summary>
        /// Distance to cull objects.
        /// </summary>
        public float distance = 15;

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
        void Start()
        {
            Camera camera = GetComponent<Camera>();
            float[] distances = new float[32];
            distances[9] = distance;
            camera.layerCullDistances = distances;
        }
    }
}