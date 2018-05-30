using System.Collections;
using System.Collections.Generic;
using UBUSetasVR.Placement;
using UnityEngine;

namespace UBUSetasVR
{
    [ExecuteInEditMode]
    public class CheckPlacementSet : MonoBehaviour
    {
        void Awake()
        {
            BasePlacement[] placements = FindObjectOfType<LevelManager>().gameObject.GetComponents<BasePlacement>();
            if(placements.Length < 1)
            {
                Debug.LogError("Se necesita al menos un Placement en el LevelManager");
            }
            else if(placements.Length > 1)
            {
                Debug.LogError("Solo puede haber un Placement al mismo tiempo en el LevelManager");
            }
        }
    }
}
