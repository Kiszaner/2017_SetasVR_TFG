using UnityEngine;

namespace UBUSetasVR
{
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

        private void OnInputModeChange(InputMode inputMode)
        {
            ChangeLaserVisibility();
        }

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
