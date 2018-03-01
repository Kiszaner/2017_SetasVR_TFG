﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database.DataStructures
{
    public class Mushroom
    {
        public string Name;
        public string Description;
        public string Species;
        public bool isEdible;
        public bool isNotEdible;
        public bool isRecommended;
        public bool isPsycotropic;
        public bool isPoisonous;
        public bool isDangerous;
        public bool isDeadly;
        public bool isUnknown;

        public Mushroom(string name, string description, string edibleConditions, string species)
        {
            Name = name;
            Description = description;
            SetEdibleConditions(edibleConditions);
            Species = species;
        }

        private void SetEdibleConditions(string edibleConditions)
        {
            string[] conditions = edibleConditions.Split(' ');
            bool notChar = false;
            for (int i = 0; i < conditions.Length; i++)
            {
                if(conditions[i] == "No")
                {
                    notChar = true;
                }
                else
                {
                    SwitchEdibleConditions(conditions[i], notChar);
                    notChar = false;
                }
            }
        }

        private void SwitchEdibleConditions(string v, bool notChar)
        {
            Debug.Log("V: " + v);
            if (notChar)
            {
                isNotEdible = true;
                return;
            }
            switch (v.ToLower())
            {
                case "comestible":
                    isEdible = true;
                    break;
                case "recomendada":
                    isRecommended = true;
                    break;
                case "psicotropica":
                    isPsycotropic = true;
                    break;
                case "peligrosa":
                    isPoisonous = true;
                    break;
                case "mortal":
                    isDeadly = true;
                    break;
                case "desconocido":
                    isUnknown = true;
                    break;
                default:
                    Debug.Log("Default: " + v);
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return string.Concat("Nombre: ", Name, ". Descripcion: ", Description, ". Species: ", Species, ". EdibleCondition: ");
        }
    }
}
