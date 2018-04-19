using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class EndGameText : MonoBehaviour
{
    public Text endGameText;
    public GameObject endGameTextParent;

    private void OnEnable()
    {
        LevelManager.OnGameOver += OnGameIsOver;
    }

    private void OnGameIsOver(bool isPlayerWinner, string endGameMessage)
    {
        if (endGameTextParent != null)
        {
            endGameTextParent.SetActive(true);
        }
        endGameText.gameObject.SetActive(true);
        endGameText.text = endGameMessage;
        if (isPlayerWinner)
        {
            endGameText.color = Color.green;
        }
        else
        {
            endGameText.color = Color.red;
        }
    }
}
