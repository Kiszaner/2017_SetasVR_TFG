using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database.DataStructures
{
    public class Mushroom
    {
        public string Name;
        public string Description;
        public EdibleType EdibleCondition;
        public string Species;

        public Mushroom(string name, string description, string edibleCondition, string species)
        {
            Name = name;
            Description = description;
            EdibleCondition = GetEdibleType(edibleCondition);
            Species = species;
        }

        EdibleType GetEdibleType(string edibleType)
        {
            // TODO localization manager
            if (true) //spanish == true
            {
                edibleType = edibleType.ToLower();
                if (edibleType.Contains("mortal"))
                {
                    return EdibleType.Deadly;
                }
                else if(edibleType.Contains("venenosa"))
                {
                    return EdibleType.Poisonous;
                }
                else if (edibleType.Contains("no comestible") || edibleType.Contains("peligrosa"))
                {
                    return EdibleType.NotEdible;
                }
                else if (edibleType.Contains("comestible"))
                {
                    return EdibleType.Edible;
                }
                else
                {
                    return EdibleType.Unknown;
                }
            }
            // TODO localization manager
            //else if (english)
        }

        public override string ToString()
        {
            return string.Concat("Nombre: ", Name, ". Descripcion: ", Description, ". EdibleCondition: ", EdibleCondition.ToString(), ". Species: ", Species);
        }
    }
}

public enum EdibleType { Edible, NotEdible, Poisonous, Deadly, Unknown };
