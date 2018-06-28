using UnityEngine;
using DaydreamElements.ObjectManipulation;

namespace UBUSetasVR.Managers
{
    /// <summary>
    /// Class that defines a manager to handle the input mode changes and triggers the proper events.
    /// </summary>
    public class InputModeManager : MonoBehaviour
    {
        /// <summary>
        /// Current input mode
        /// </summary>
        public static InputMode currentInputMode;

        /// <summary>
        /// Trigger used to switch to teleport input.
        /// </summary>
        [Tooltip("Trigger used to switch to teleport input")]
        public BaseActionTrigger teleportStartTrigger;

        /// <summary>
        /// Delegate to handle input mode changes.
        /// </summary>
        /// <param name="newInputMode">The new input mode</param>
        public delegate void InputModeChange(InputMode newInputMode);

        /// <summary>
        /// Event fired after the input mode has been changed.
        /// </summary>
        public static event InputModeChange OnInputModeChange;

        /// <summary>
        /// Trigger used to switch to manipulation input.
        /// </summary>
        [Tooltip("Trigger used to switch to manipulation input")]
        public BaseActionTrigger manipulationStartTrigger;

        /// <summary>
        /// Previous input mode.
        /// </summary>
        private InputMode previousInputMode;

        /// <summary>
        /// Unity method that runs every frame.
        /// </summary>
        void Update()
        {
            if (manipulationStartTrigger.TriggerActive())
            {
                if (previousInputMode != InputMode.MANIPULATION)
                {
                    currentInputMode = InputMode.MANIPULATION;
                    previousInputMode = InputMode.MANIPULATION;
                    if (OnInputModeChange != null)
                    {
                        OnInputModeChange(InputMode.MANIPULATION);
                    }
                }
                return;
            }
            if (teleportStartTrigger.TriggerActive())
            {
                if (previousInputMode == InputMode.MANIPULATION)
                {
                    if (ObjectManipulationPointer.IsObjectSelected())
                    {
                        return;
                    }
                }
                if (previousInputMode != InputMode.TELEPORT)
                {
                    currentInputMode = InputMode.TELEPORT;
                    previousInputMode = InputMode.TELEPORT;
                    if (OnInputModeChange != null)
                    {
                        OnInputModeChange(InputMode.TELEPORT);
                    }
                }
                return;
            }
        }
    }

    /// <summary>
    /// Enumeration to keep input mode status easier.
    /// </summary>
    public enum InputMode { MANIPULATION, TELEPORT };
}