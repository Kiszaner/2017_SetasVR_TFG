using DaydreamElements.ObjectManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelHandler : MonoBehaviour
{
    public GameObject InfoPanel;
    private MoveablePhysicsObject MPO;
    public BaseActionTrigger informationPanelToggleTrigger;

    // Use this for initialization
    void Start ()
    {
        MPO = GetComponent<MoveablePhysicsObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (MPO.State == BaseInteractiveObject.ObjectState.Selected)
        {
            if (informationPanelToggleTrigger.TriggerActive())
            {
                InfoPanel.SetActive(!InfoPanel.activeSelf);
            }
        }
	}
}
