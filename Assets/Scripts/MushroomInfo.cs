using System.Collections;
using System.Collections.Generic;
using UBUSetasVR;
using UnityEngine;
using UnityEngine.UI;

public class MushroomInfo : MonoBehaviour
{
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
    void Start ()
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
        //EdibleText.text = Mushroom.IsEdible.ToString();
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
