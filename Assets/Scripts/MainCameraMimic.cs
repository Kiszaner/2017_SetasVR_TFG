using UnityEngine;

namespace UBUSetasVR
{
    public class MainCameraMimic : MonoBehaviour
    {
        public Camera mainCamera;
        // Use this for initialization
        void Start()
        {
            if (mainCamera == null)
            {
                UpdateCameraRef();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (mainCamera != null)
            {
                transform.position = mainCamera.transform.position;
                transform.rotation = mainCamera.transform.rotation;
            }
        }

        public bool UpdateCameraRef()
        {
            mainCamera = Camera.main;
            return mainCamera != null;
        }
    }

    public static class MainCameraMimicExtension
    {
        public static bool UpdateMainCameraRef(this MainCameraMimic[] mimics)
        {
            bool res = false;
            foreach (MainCameraMimic mimic in mimics)
            {
                res = mimic.UpdateCameraRef();
                if (res == false)
                {
                    return res;
                }
            }

            return res;
        }
    }
}