using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR.UI
{
    /// <summary>
    /// Class to handle switch toggles in the information panels.
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleSwitch : MonoBehaviour
    {
        /// <summary>
        /// Background element of the toggle.
        /// </summary>
        public GameObject backgroundCross;

        // Use this for initialization
        void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(Switch);
        }

        /// <summary>
        /// Switches the active status of the toogle.
        /// </summary>
        /// <param name="on">Is the toogle on?</param>
        private void Switch(bool on)
        {
            backgroundCross.SetActive(!on);
        }
    }
}