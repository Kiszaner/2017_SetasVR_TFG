using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MushroomList", menuName = "UBUSetas/MushroomList", order = 2)]
    public class MushroomList : ScriptableObject
    {
        public List<MushroomScriptableObject> mushroomList;
    }
}