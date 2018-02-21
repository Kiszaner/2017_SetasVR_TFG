using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MushroomData", menuName = "UBUSetas/MushroomData", order = 1)]
public class MushroomScriptableObject : ScriptableObject
{
    public string Name;
    public string Description;
    public string Species;
    public EdibleType EdibleCondition;
}
