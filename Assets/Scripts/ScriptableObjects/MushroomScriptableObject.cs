using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database.DataStructures;

[CreateAssetMenu(fileName = "MushroomData", menuName = "UBUSetas/MushroomData", order = 1)]
public class MushroomScriptableObject : ScriptableObject
{
    public string Name;
    public string Description;
    public string Species;
    public int ScoreValue = 5;
    public bool IsEdible;
    public bool IsNotEdible;
    public bool IsRecommended;
    public bool IsPsycotropic;
    public bool IsPoisonous;
    public bool IsDangerous;
    public bool IsDeadly;
    public bool IsUnknown;
    public Sprite[] Photos;

    public void DataToScriptable(Mushroom m)
    {
        Name = m.Name;
        Description = m.Description;
        Species = m.Species;
        SetEdibleConditions(m.isEdible, m.isNotEdible, m.isRecommended, m.isPsycotropic, m.isPoisonous, m.isDangerous, m.isDeadly, m.isUnknown);
    }

    private void SetEdibleConditions(bool isEdible, bool isNotEdible, bool isRecommended, bool isPsycotropic, bool isPoisonous, bool isDangerous, bool isDeadly, bool isUnknown)
    {
        IsEdible = isEdible;
        IsNotEdible = isNotEdible;
        IsRecommended = isRecommended;
        IsPsycotropic = isPsycotropic;
        IsDangerous = isDangerous;
        IsDeadly = isDeadly;
        IsUnknown = isUnknown;
    }
}
