using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomPlacement))]
public class LevelManager : MonoBehaviour
{
    public bool autoPlaceEnvironment = false;
    public int targetScore = 10;
    public float timeLimit = 120f;
    [Tooltip("Número de setas recogidas acertadas para acabar el nivel")]
    public int targetNumSuccess = 3;
    [Tooltip("Número de setas a recoger para acabar el nivel")]
    public int targetNumMushrooms = 4;
    public int maxNumTries = 6;
    public bool targetScoreEnabled = true;
    public bool timeLimitEnabled = true;
    private bool numSuccessEnabled = true;
    private bool numMushroomsEnable = true;
    public bool maxTriesEnabled = true;
    public delegate void ScoreUpdateDelegate(int value);
    public static event ScoreUpdateDelegate OnScoreUpdated;
    public delegate void GameOverDelegate(bool isPlayerWinner, string endGameText);
    public static event GameOverDelegate OnGameOver;

    [SerializeField]
    private int currentScore = 0;
    private float timeLeft;
    private int currentNumSuccess = 0;
    private int currentNumMushrooms = 0;
    private int triesLeft;
    private RandomPlacement placer;
    private bool gameOver = false;
    private string endGameText;

    private void OnEnable()
    {
        Basket.OnMushroomInBasket += MushroomInBasket;
    }

    private void OnDisable()
    {
        Basket.OnMushroomInBasket -= MushroomInBasket;
    }

    void Start ()
    {
        placer = GetComponent<RandomPlacement>();
        ResetScore();
        timeLeft = timeLimit;
        triesLeft = maxNumTries;
        if (autoPlaceEnvironment)
        {
            placer.RepeatAllPlacement();
        }
    }
	
	void Update ()
    {
        if (gameOver) return;
        if (timeLimitEnabled)
        {
            timeLeft -= Time.deltaTime;
            CheckTimeCondition();
        }
    }

    private void CheckTimeCondition()
    {
        if (timeLeft <= 0)
        {
            Debug.Log("Time's up!");
            endGameText = "Se ha terminado el tiempo";
            PlayerLose();
        }
    }

    private void MushroomInBasket(GameObject mushroom, bool success, int value)
    {
        Debug.Log("MushroomInBasket");
        if (gameOver) {
            Debug.Log("Game is over, no further conditions will be checked");
            Destroy(mushroom);
            return;
        }
        UpdateVariables(mushroom, success);
        UpdateScore(success, value);
        CheckWinConditions();
        CheckLoseConditions();
    }

    private void UpdateVariables(GameObject mushroom, bool success)
    {
        Debug.Log("UpdateVariables");
        if (success)
        {
            currentNumSuccess++;
        }
        currentNumMushrooms++;
        triesLeft--;
        placer.RemoveMushroomFromList(mushroom);
    }

    private void UpdateScore(bool success, int value)
    {
        Debug.Log("UpdateScore");
        if (success)
        {
            currentScore += value;
        }
        else
        {
            currentScore -= value;
        }
        UpdateScoreCounter();
    }

    private void ResetScore()
    {
        Debug.Log("ResetScore");
        currentScore = 0;
        UpdateScoreCounter();
    }

    private void UpdateScoreCounter()
    {
        Debug.Log("UpdateScoreCounter");
        if (OnScoreUpdated != null)
        {
            OnScoreUpdated(currentScore);
        }
    }

    private void CheckWinConditions()
    {
        Debug.Log("CheckWinConditions");
        if (targetScoreEnabled)
        {
            if (currentScore >= targetScore)
            {
                Debug.Log("Target score reached!");
                endGameText = "Puntuación objetivo conseguida";
                PlayerWin();
            }
        }
        if (numSuccessEnabled)
        {
            if(currentNumSuccess >= targetNumSuccess)
            {
                Debug.Log("Target num of success reached!");
                endGameText = "Acertadas las setas necesarias";
                PlayerWin();
            }
        }
        if (numMushroomsEnable)
        {
            if(currentNumMushrooms >= targetNumMushrooms)
            {
                Debug.Log("Target num of mushrooms reached!");
                endGameText = "Encontradas todas las setas necesarias";
                PlayerWin();
            }
        }
    }

    private void CheckLoseConditions()
    {
        Debug.Log("CheckLoseConditions");
        if (maxTriesEnabled)
        {
            if(triesLeft <= 0)
            {
                Debug.Log("Max num of tries reached!");
                endGameText = "Número máximo de intentos alcanzado";
                PlayerLose();
            }
        }
        // Time condition checked on Update
    }

    private void PlayerWin()
    {
        Debug.Log("PlayerWin");
        endGameText = endGameText + ", has ganado!";
        TriggerEnd(true);
    }

    private void PlayerLose()
    {
        Debug.Log("PlayerLose");
        endGameText = endGameText + ", has perdido!";
        TriggerEnd(false);
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
        gameOver = true;
    }

    private void TriggerEnd(bool isPlayerWinner)
    {
        GameOver();
        if (OnGameOver != null)
        {
            OnGameOver(isPlayerWinner, endGameText);
        }
    }
}
