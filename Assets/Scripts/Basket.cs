using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public bool IsGoodBasket;
    public delegate void MushroomInBasket(bool success, int value); 
    public static event MushroomInBasket OnMushroomInBasket;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mushroom"))
        {
            MushroomInfo info = other.gameObject.GetComponent<MushroomInfo>();
            if(info != null)
            {
                CheckConditions(info);
                Destroy(other.gameObject);
            }
        }
        else
        {
            Debug.Log("No es una seta");
        }
    }

    private void CheckConditions(MushroomInfo info)
    {
        if (info.Mushroom.IsEdible || info.Mushroom.IsRecommended)
        {
            Debug.Log("Comestible o recomendada");
            if (IsGoodBasket)
            {
                OnMushroomInBasket(true, info.Mushroom.ScoreValue);
            }
            else
            {
                OnMushroomInBasket(false, info.Mushroom.ScoreValue);
            }
        }
        else
        {
            Debug.Log("Otra cosa");
            if (IsGoodBasket)
            {
                OnMushroomInBasket(false, info.Mushroom.ScoreValue);
            }
            else
            {
                OnMushroomInBasket(true, info.Mushroom.ScoreValue);
            }
        }
    }

    public static void RaiseEvent(bool success, int value)
    {
        OnMushroomInBasket(success, value);
    }
}
