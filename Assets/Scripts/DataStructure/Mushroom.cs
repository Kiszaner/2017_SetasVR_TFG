using System;
using UnityEngine;

namespace UBUSetasVR.Database.DataStructures
{
    /// <summary>
    /// Class that defines the data structure of a mushroom from the database.
    /// </summary>
    public class Mushroom
    {
        /// <summary>
        /// Name of the mushroom.
        /// </summary>
        public string name;

        /// <summary>
        /// Description of the mushroom.
        /// </summary>
        public string description;

        /// <summary>
        /// Specie of the mushroom.
        /// </summary>
        public string species;

        /// <summary>
        /// Is edible the mushroom?
        /// </summary>
        public bool isEdible;

        /// <summary>
        /// Is not edible the mushroom?
        /// </summary>
        public bool isNotEdible;

        /// <summary>
        /// Is recommended the mushroom?
        /// </summary>
        public bool isRecommended;

        /// <summary>
        /// Is psycotropic the mushroom?
        /// </summary>
        public bool isPsycotropic;

        /// <summary>
        /// Is poisonous the mushroom?
        /// </summary>
        public bool isPoisonous;

        /// <summary>
        /// Is dangerous the mushroom?
        /// </summary>
        public bool isDangerous;

        /// <summary>
        /// Is deadly the mushroom?
        /// </summary>
        public bool isDeadly;

        /// <summary>
        /// Is unknown or complex the edibility of the mushroom?
        /// </summary>
        public bool isUnknown;

        /// Initializes a new instance of the <see cref="UBUSetasVR.Database.DataStructures.Mushroom" /> class. 
        /// </summary>
        /// <param name="name">Name of the mushroom</param>
        /// <param name="description">Description of the mushroom</param>
        /// <param name="edibleConditions">Edible conditions of the mushroom</param>
        /// <param name="species">Specie of the mushroom</param>
        public Mushroom(string name, string description, string edibleConditions, string species)
        {
            this.name = name;
            this.description = description;
            SetEdibleConditions(edibleConditions);
            this.species = species;
        }

        /// <summary>
        /// Method that splits the edible conditions of the mushroom to convert them to simpler processing variables.
        /// </summary>
        /// <param name="edibleConditions">Edible conditions of the mushroom</param>
        private void SetEdibleConditions(string edibleConditions)
        {
            // Splits the edible conditions from the string
            string[] conditions = edibleConditions.Split(' ');
            bool notChar = false;
            string conditionString;
            for (int i = 0; i < conditions.Length; i++)
            {
                conditionString = conditions[i].ToLower();
                // Take care of the "not" particle in the not edible mushrooms
                if (conditionString == "no")
                {
                    notChar = true;
                }
                else
                {
                    SwitchEdibleConditions(conditionString, notChar);
                    notChar = false;
                }
            }
        }

        /// <summary>
        /// Method that saves an edible condition to the corresponding simpler variable of
        /// the mushroom.
        /// </summary>
        /// <remarks>
        /// This methods currently only works with spanish strings.
        /// </remarks>
        /// <param name="edibleCondition"{additionalAttributes}>String with the edible
        /// condition</param>
        /// <param name="notChar"{additionalAttributes}>If set to <see langword="true"/>,
        /// then the string has a &quot;not&quot; particle in it and is a not edible
        /// condition; otherwise processes it normally.</param>
        private void SwitchEdibleConditions(string edibleCondition, bool notChar)
        {
            if (notChar)
            {
                isNotEdible = true;
                return;
            }
            switch (edibleCondition.ToLower())
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
                    isDangerous = true;
                    break;
                case "venenosa":
                    isPoisonous = true;
                    break;
                case "mortal":
                    isDeadly = true;
                    break;
                case "desconocido":
                    isUnknown = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Auxiliar method to print mushrooms values in the console, except the edible conditions.
        /// </summary>
        /// <returns>The values of the mushroom except the edible conditions</returns>
        public override string ToString()
        {
            return string.Concat("Nombre: ", name, ". Descripcion: ", description, ". Species: ", species, ". EdibleCondition: ");
        }
    }
}
