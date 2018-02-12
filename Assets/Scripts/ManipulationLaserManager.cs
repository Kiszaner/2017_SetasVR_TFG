using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaydreamElements.Teleport;
using System;

public class ManipulationLaserManager : MonoBehaviour
{
    public GameObject manipulationLaser;

    private void OnEnable()
    {
        InputModeManager.OnInputModeChange += OnInputModeChange;
    }

    private void OnDisable()
    {
        InputModeManager.OnInputModeChange -= OnInputModeChange;
    }

    private void OnInputModeChange()
    {
        ChangeLaserVisibility();
    }

    private void ChangeLaserVisibility()
    {
        if (InputModeManager.currentInputMode != InputModeManager.InputMode.MANIPULATION)
        {
            manipulationLaser.SetActive(false);
        }
        else
        {
            manipulationLaser.SetActive(true);
        }
    }
}
