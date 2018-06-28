using UnityEngine;

namespace UBUSetasVR.Managers
{
    /// <summary>
    /// Class that handles the controller tooltips.
    /// </summary>
    public class TooltipManager : MonoBehaviour
    {
        /// <summary>
        /// Tooltip instance for teleport mode.
        /// </summary>
        private GameObject teleportTooltip;

        /// <summary>
        /// Tooltip instance for the manipulation mode.
        /// </summary>
        private GameObject manipulationTooltip;

        /// <summary>
        /// Unity method that runs everytime the GameObject is enabled in the inspector.
        /// </summary>
        private void OnEnable()
        {
            InputModeManager.OnInputModeChange += OnInputModeChange;
        }

        /// <summary>
        /// Unity method that runs everytime the GameObject is diabled in the inspector.
        /// </summary>
        private void OnDisable()
        {
            InputModeManager.OnInputModeChange -= OnInputModeChange;
        }

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
        void Start()
        {
            foreach(Transform tf in transform.GetComponentsInChildren<Transform>())
            {
                if (tf.CompareTag("ManipulationElement"))
                {
                    manipulationTooltip = tf.gameObject;
                }
                else if (tf.CompareTag("TeleportElement"))
                {
                    teleportTooltip = tf.gameObject;
                }
            }
            OnInputModeChange(InputModeManager.currentInputMode);
        }

        /// <summary>
        /// Handles the input mode changed event. It sets the proper tooltip active or inactive, alternating the tooltips.
        /// </summary>
        /// <param name="newInputMode"></param>
        private void OnInputModeChange(InputMode newInputMode)
        {
            if(newInputMode == InputMode.MANIPULATION)
            {
                teleportTooltip.SetActive(false);
                manipulationTooltip.SetActive(true);
            }
            else if(newInputMode == InputMode.TELEPORT)
            {
                teleportTooltip.SetActive(true);
                manipulationTooltip.SetActive(false);
            }
        }
    }
}