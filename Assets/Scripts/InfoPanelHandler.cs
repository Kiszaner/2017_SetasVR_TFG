using DaydreamElements.ObjectManipulation;
using UBUSetasVR.UI;
using UnityEngine;

namespace UBUSetasVR
{
    /// <summary>
    /// Class that defines a handler for a information panel.
    /// </summary>
    public class InfoPanelHandler : MonoBehaviour
    {
        /// <summary>
        /// Instance of the information panel.
        /// </summary>
        public GameObject InfoPanel;

        /// <summary>
        /// Trigger of the information panel toggle.
        /// </summary>
        public BaseActionTrigger informationPanelToggleTrigger;

        /// <summary>
        /// Object where this information panel is attached to.
        /// </summary>
        private MoveablePhysicsObject MPO;

        /// <summary>
        /// Information of the mushroom.
        /// </summary>
        private MushroomInfo mushroomInfo;

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
        void Start()
        {
            MPO = GetComponent<MoveablePhysicsObject>();
            mushroomInfo = GetComponent<MushroomInfo>();
        }

        /// <summary>
        /// Unity method that runs every frame.
        /// </summary>
        void Update()
        {
            if (MPO.State == BaseInteractiveObject.ObjectState.Selected)
            {
                if (informationPanelToggleTrigger.TriggerActive())
                {
                    InfoPanel.SetActive(!InfoPanel.activeSelf);
                    mushroomInfo.infoAlreadyColsulted = true;
                }
            }
            else if (MPO.State == BaseInteractiveObject.ObjectState.Released)
            {
                InfoPanel.SetActive(false);
            }
        }
    }
}