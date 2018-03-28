using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public bool IsGoodBasket;
    public delegate void MushroomInBasket(GameObject mushroom, bool success, int value); 
    public static event MushroomInBasket OnMushroomInBasket;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mushroom"))
        {
            GameObject mushroom = other.gameObject;
            MushroomInfo info = mushroom.GetComponent<MushroomInfo>();
            if(info != null)
            {
                CheckConditions(info, mushroom);
                //Destroy(other.gameObject);
            }
        }
        else
        {
            Debug.Log("No es una seta");
        }
    }

    private void CheckConditions(MushroomInfo info, GameObject mushroom)
    {
        if (info.Mushroom.IsEdible || info.Mushroom.IsRecommended)
        {
            Debug.Log("Comestible o recomendada");
            if (IsGoodBasket)
            {
                OnMushroomInBasket(mushroom, true, info.Mushroom.ScoreValue);
            }
            else
            {
                OnMushroomInBasket(mushroom, false, info.Mushroom.ScoreValue);
            }
        }
        else
        {
            Debug.Log("Otra cosa");
            if (IsGoodBasket)
            {
                OnMushroomInBasket(mushroom, false, info.Mushroom.ScoreValue);
            }
            else
            {
                OnMushroomInBasket(mushroom, true, info.Mushroom.ScoreValue);
            }
        }
    }
}
