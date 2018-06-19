using UnityEngine;
using UnityEngine.UI;
using UBUSetasVR.ScriptableObjects;

namespace UBUSetasVR
{
    public class MushroomInfo : MonoBehaviour
    {
        public bool infoAlreadyColsulted;
        public GameObject canvas;
        public MushroomScriptableObject Mushroom;
        public Text nameText;
        public Text DescriptionText;
        public Toggle IsEdible;
        public Toggle IsNotEdible;
        public Toggle IsRecommended;
        public Toggle IsPsycotropic;
        public Toggle IsPoisonous;
        public Toggle IsDangerous;
        public Toggle IsDeadly;
        public Toggle IsUnknown;
        public Text SpeciesText;
        public RawImage[] PhotosTexture;

        // Use this for initialization
        void Start()
        {
            // Need canvas active at the start to update toggles status
            canvas.SetActive(true);
            nameText.text = AuxiliarFunctions.FirstUpper(Mushroom.Name);
            DescriptionText.text = Mushroom.Description;
            IsEdible.isOn = Mushroom.IsEdible;
            IsNotEdible.isOn = Mushroom.IsNotEdible;
            IsRecommended.isOn = Mushroom.IsRecommended;
            IsPsycotropic.isOn = Mushroom.IsPsycotropic;
            IsPoisonous.isOn = Mushroom.IsPoisonous;
            IsDangerous.isOn = Mushroom.IsDangerous;
            IsDeadly.isOn = Mushroom.IsDeadly;
            IsUnknown.isOn = Mushroom.IsUnknown;
            SpeciesText.text = Mushroom.Species;
            if (Mushroom.Photos.Length != 0)
            {
                for (int i = 0; i < PhotosTexture.Length; i++)
                {
                    PhotosTexture[i].texture = Mushroom.Photos[i];
                }
            }
            else
            {
                for (int i = 0; i < PhotosTexture.Length; i++)
                {
                    PhotosTexture[i].enabled = false;
                }
            }
            canvas.SetActive(false);
        }
    }
}