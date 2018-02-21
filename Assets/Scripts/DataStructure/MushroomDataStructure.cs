using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDataStructure
{
    public string Name;
    public string Description;
    public EdibleType EdibleCondition;
    public string Species;

    public MushroomDataStructure(string name, string description, EdibleType edibleCondition, string species)
    {
        Name = name;
        Description = description;
        EdibleCondition = edibleCondition;
        Species = species;
    }
}

public enum EdibleType { Edible, NotEdible, Poisonous, Deadly, Unknown };
