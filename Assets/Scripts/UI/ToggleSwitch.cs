using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleSwitch : MonoBehaviour
    {
        public GameObject backgroundCross;

        // Use this for initialization
        void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(Switch);
        }

        private void Switch(bool on)
        {
            backgroundCross.SetActive(!on);
        }
    }
}