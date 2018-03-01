using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MushroomInfo : MonoBehaviour
{
    public MushroomScriptableObject Mushroom;
    public Text nameText;
    public Text DescriptionText;
    public Text EdibleText;
    public Text SpeciesText;
    public Sprite[] PhotosSprites;

    // Use this for initialization
    void Start ()
    {
        nameText.text = Mushroom.Name;
        DescriptionText.text = Mushroom.Description;
        EdibleText.text = Mushroom.IsEdible.ToString();
        SpeciesText.text = Mushroom.Species;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
