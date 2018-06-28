using UnityEngine;

namespace UBUSetasVR.Managers
{
    /// <summary>
    /// Class that handles the change of the manipulation laser status and related laser stuff.
    /// </summary>
    public class ManipulationLaserManager : MonoBehaviour
    {
        /// <summary>
        /// Instance of the manipulation laser.
        /// </summary>
        public GameObject manipulationLaser;


        /// <summary>
        /// Unity method that runs everytime the GameObject is enabled in the inspector.
        /// </summary>
        private void OnEnable()
        {
            InputModeManager.OnInputModeChange += OnInputModeChange;
        }

        /// <summary>
        /// Unity method that runs everytime the GameObject is disabled in the inspector.
        /// </summary>
        private void OnDisable()
        {
            InputModeManager.OnInputModeChange -= OnInputModeChange;
        }

        /// <summary>
        /// Handles the input mode changed event.
        /// </summary>
        /// <param name="inputMode"></param>
        private void OnInputModeChange(InputMode inputMode)
        {
            ChangeLaserVisibility();
        }

        /// <summary>
        /// Changes the laser visibility based on the current input mode.
        /// </summary>
        private void ChangeLaserVisibility()
        {
            if (InputModeManager.currentInputMode != InputMode.MANIPULATION)
            {
                manipulationLaser.SetActive(false);
            }
            else
            {
                manipulationLaser.SetActive(true);
            }
        }
    }
}
