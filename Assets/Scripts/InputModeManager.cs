using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaydreamElements.ObjectManipulation;

public class InputModeManager : MonoBehaviour
{
    public enum InputMode { MANIPULATION, TELEPORT};
    public static InputMode currentInputMode;
    /// Trigger used to switch to teleport input
    [Tooltip("Trigger used to switch to teleport input")]
    public BaseActionTrigger teleportStartTrigger;
    public delegate void InputModeChange();
    public static event InputModeChange OnInputModeChange;

    /// Trigger used to switch to manipulation input
    [Tooltip("Trigger used to switch to manipulation input")]
    public BaseActionTrigger manipulationStartTrigger;

    private InputMode previousInputMode;

    // Update is called once per frame
    void Update ()
    {
        if (manipulationStartTrigger.TriggerActive())
        {
            if (previousInputMode != InputMode.MANIPULATION)
            {
                currentInputMode = InputMode.MANIPULATION;
                previousInputMode = InputMode.MANIPULATION;
                if (OnInputModeChange != null)
                {
                    OnInputModeChange();
                }
            }
            return;
        }
        if (teleportStartTrigger.TriggerActive())
        {
            if(previousInputMode == InputMode.MANIPULATION)
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
                    OnInputModeChange();
                }
            }
            return;
        }
	}
}
