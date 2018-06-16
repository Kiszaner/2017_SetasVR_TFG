using DaydreamElements.ObjectManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelHandler : MonoBehaviour
{
    public GameObject InfoPanel;
    public BaseActionTrigger informationPanelToggleTrigger;

    private MoveablePhysicsObject MPO;
    private MushroomInfo mushroomInfo;

    // Use this for initialization
    void Start ()
    {
        MPO = GetComponent<MoveablePhysicsObject>();
        mushroomInfo = GetComponent<MushroomInfo>();
	}
	
	// Update is called once per frame
	void Update ()
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
