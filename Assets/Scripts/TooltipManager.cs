using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR
{
    public class TooltipManager : MonoBehaviour
    {
        private GameObject teleportTooltip;
        private GameObject manipulationTooltip;

        private void OnEnable()
        {
            InputModeManager.OnInputModeChange += OnInputModeChange;
        }

        private void OnDisable()
        {
            InputModeManager.OnInputModeChange -= OnInputModeChange;
        }

        // Use this for initialization
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