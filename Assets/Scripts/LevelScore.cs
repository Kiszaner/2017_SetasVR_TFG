using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScore : MonoBehaviour
{
    public Text ScoreText;
    public int Score = 0;

    private void OnEnable()
    {
        Basket.OnMushroomInBasket += UpdateScore;
    }

    private void OnDisable()
    {
        Basket.OnMushroomInBasket -= UpdateScore;
    }

    private void Start()
    {
        ScoreText.text = "Puntuación: " + Score;
    }

    private void UpdateScore(bool success, int value = 0)
    {
        Debug.Log("UpdateScore: "+value+". Current Score: "+Score);
        if (success)
        {
            Score += value;
        }
        else
        {
            Score -= value;
        }
        WriteCurrentScore();
    }

    private void WriteCurrentScore()
    {
        ScoreText.text = "Puntuación: " + Score;
    }

    public void ResetScore()
    {
        Score = 0;
        WriteCurrentScore();
    }
}
