using UnityEngine;
using UBUSetasVR.Database.DataStructures;

namespace UBUSetasVR.ScriptableObjects
{
    /// <summary>
    /// Class that defines the data container of a mushroom to be used in Unity's editor.
    /// </summary>
    [CreateAssetMenu(fileName = "MushroomData", menuName = "UBUSetas/MushroomData", order = 1)]
    public class MushroomScriptableObject : ScriptableObject
    {
        /// <summary>
        /// Name of the mushroom.
        /// </summary>
        public string Name;

        /// <summary>
        /// Description of the mushroom.
        /// </summary>
        public string Description;

        /// <summary>
        /// Species of the mushroom.
        /// </summary>
        public string Species;

        /// <summary>
        /// Value of the mushroom.
        /// </summary>
        public int ScoreValue = 5;

        /// <summary>
        /// Is the mushroom edible?
        /// </summary>
        public bool IsEdible;

        /// <summary>
        /// Is the mushroom not edible?
        /// </summary>
        public bool IsNotEdible;

        /// <summary>
        /// Is the mushroom recommended?
        /// </summary>
        public bool IsRecommended;

        /// <summary>
        /// Is the mushroom psycotropic?
        /// </summary>
        public bool IsPsycotropic;

        /// <summary>
        /// Is the mushroom poisonous?
        /// </summary>
        public bool IsPoisonous;

        /// <summary>
        /// Is the mushroom dangerous?
        /// </summary>
        public bool IsDangerous;

        /// <summary>
        /// Is the mushroom deadly?
        /// </summary>
        public bool IsDeadly;

        /// <summary>
        /// Is the mushroom edibility unknown or complex?
        /// </summary>
        public bool IsUnknown;

        /// <summary>
        /// Real photos of the mushroom
        /// </summary>
        public Texture[] Photos;

        /// <summary>
        /// Passes the data structure of a mushroom into the container of data.
        /// </summary>
        /// <param name="m">Data structure of the mushroom</param>
        public void DataToScriptable(Mushroom m)
        {
            Name = m.name;
            Description = m.description;
            Species = m.species;
            SetEdibleConditions(m.isEdible, m.isNotEdible, m.isRecommended, m.isPsycotropic, m.isPoisonous, m.isDangerous, m.isDeadly, m.isUnknown);
        }

        /// <summary>
        /// Sets the data container edible conditions based on arguments.
        /// </summary>
        /// <param name="isEdible">Is the mushroom edible?</param>
        /// <param name="isNotEdible">Is the mushroom not edible?</param>
        /// <param name="isRecommended">Is the mushroom recommended?</param>
        /// <param name="isPsycotropic">Is the mushroom psycotropic?</param>
        /// <param name="isPoisonous">Is the mushroom poisonous?</param>
        /// <param name="isDangerous">Is the mushroom dangerous?</param>
        /// <param name="isDeadly">Is the mushroom deadly?</param>
        /// <param name="isUnknown">Is the mushroom edibility unknown or complex?</param>
        private void SetEdibleConditions(bool isEdible, bool isNotEdible, bool isRecommended, bool isPsycotropic, bool isPoisonous, bool isDangerous, bool isDeadly, bool isUnknown)
        {
            IsEdible = isEdible;
            IsNotEdible = isNotEdible;
            IsRecommended = isRecommended;
            IsPsycotropic = isPsycotropic;
            IsPoisonous = isPoisonous;
            IsDangerous = isDangerous;
            IsDeadly = isDeadly;
            IsUnknown = isUnknown;
        }
    }
}