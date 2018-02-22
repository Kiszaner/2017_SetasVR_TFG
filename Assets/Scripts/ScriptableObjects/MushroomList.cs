using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MushroomList", menuName = "UBUSetas/MushroomList", order = 2)]
public class MushroomList : ScriptableObject
{
    public List<MushroomScriptableObject> mushroomList;
}
