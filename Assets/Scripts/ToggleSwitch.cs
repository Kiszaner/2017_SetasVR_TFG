using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleSwitch : MonoBehaviour
    {
        public GameObject backgroundCross;

        // Use this for initialization
        void Start()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(Switch);
        }

        private void Switch(bool on)
        {
            backgroundCross.SetActive(!on);
        }
    }
}