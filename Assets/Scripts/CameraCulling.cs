using UnityEngine;

namespace UBUSetasVR
{
    public class CameraCulling : MonoBehaviour
    {
        public float distance = 15;

        // Use this for initialization
        void Start()
        {
            Camera camera = GetComponent<Camera>();
            float[] distances = new float[32];
            distances[9] = distance;
            camera.layerCullDistances = distances;
        }
    }
}