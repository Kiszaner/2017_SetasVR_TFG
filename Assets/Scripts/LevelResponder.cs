using DaydreamElements.Main;
using DaydreamElements.ObjectManipulation;
using DaydreamElements.Teleport;
using UnityEngine.Assertions;

namespace UBUSetasVR
{
    /// <summary>
    /// Class that defines certain routines to execute at the beginning of levels.
    /// </summary>
    public class LevelResponder : BaseLevelSelectResponder
    {
        /// <summary>
        /// Controller.
        /// </summary>
        private GvrTrackedController controller;

        /// <summary>
        /// Teleport controller.
        /// </summary>
        private TeleportController teleport;

        /// <summary>
        /// Manipulator pointer.
        /// </summary>
        private ObjectManipulationPointer pointer;

        /// <summary>
        /// Runs on pause menu opened.
        /// </summary>
        public override void OnMenuOpened()
        {
            SetTeleportModeEnabled(false);
            SetObjectManipulationEnabled(false);
        }

        /// <summary>
        /// Runs on pause menu closed.
        /// </summary>
        public override void OnMenuClosed()
        {
            SetTeleportModeEnabled(true);
            SetObjectManipulationEnabled(true);
        }

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
        void Start()
        {
            bool mimicsRes = false;
            controller = SceneHelpers.FindObjectOfType<GvrTrackedController>(true);
            Assert.IsNotNull(controller);
            teleport = SceneHelpers.FindObjectOfType<TeleportController>(true);
            Assert.IsNotNull(teleport);
            pointer = controller.GetComponentInChildren<ObjectManipulationPointer>(true);
            Assert.IsNotNull(pointer);
            mimicsRes = FindObjectsOfType<MainCameraMimic>().UpdateMainCameraRef();
            Assert.IsTrue(mimicsRes);
        }

        /// <summary>
        /// Unity method that runs every frame.
        /// </summary>
        void Update()
        {
            if (teleport == null)
            {
                return;
            }

            if (LevelSelectController.Instance == null)
            {
                return;
            }

            bool isTeleporting = teleport.IsSelectingTeleportLocation || teleport.IsTeleporting;
            LevelSelectController.Instance.enabled = !isTeleporting;
        }

        /// <summary>
        /// Sets the teleport elements active or inactive.
        /// </summary>
        /// <param name="enabled">Is teleport mode enabled?</param>
        private void SetTeleportModeEnabled(bool enabled)
        {
            controller.gameObject.SetActive(enabled);

            if (teleport != null)
            {
                teleport.gameObject.SetActive(enabled);
            }
        }

        /// <summary>
        /// Sets the manipulation elements active or inactive.
        /// </summary>
        /// <param name="enabled"></param>
        private void SetObjectManipulationEnabled(bool enabled)
        {
            controller.gameObject.SetActive(enabled);
            if (enabled)
            {
                GvrPointerInputModule.Pointer = pointer;
            }
        }
    }
}