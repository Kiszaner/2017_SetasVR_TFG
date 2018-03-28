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
    public delegate void GameOverDelegate(bool isPlayerWinner);
    public static event GameOverDelegate OnGameOver;

    [SerializeField]
    private int currentScore = 0;
    private float timeLeft;
    private int currentNumSuccess = 0;
    private int currentNumMushrooms = 0;
    private int triesLeft;
    private RandomPlacement placer;
    private bool gameOver = false;

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
            PlayerLose();
        }
    }

    private void MushroomInBasket(GameObject mushroom, bool success, int value)
    {
        if (gameOver) return;
        UpdateVariables(mushroom, success);
        UpdateScore(success, value);
        CheckWinConditions();
        CheckLoseConditions();
    }

    private void UpdateVariables(GameObject mushroom, bool success)
    {
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
        currentScore = 0;
        UpdateScoreCounter();
    }

    private void UpdateScoreCounter()
    {
        if (OnScoreUpdated != null)
        {
            OnScoreUpdated(currentScore);
        }
    }

    private void CheckWinConditions()
    {
        if (targetScoreEnabled)
        {
            if (currentScore >= targetScore)
            {
                Debug.Log("Target score reached!");
                PlayerWin();
            }
        }
        if (numSuccessEnabled)
        {
            if(currentNumSuccess >= targetNumSuccess)
            {
                Debug.Log("Target num of success reached!");
                PlayerWin();
            }
        }
        if (numMushroomsEnable)
        {
            if(currentNumMushrooms >= targetNumMushrooms)
            {
                Debug.Log("Target num of mushrooms reached!");
                PlayerWin();
            }
        }
    }

    private void CheckLoseConditions()
    {
        if (maxTriesEnabled)
        {
            if(triesLeft <= 0)
            {
                Debug.Log("Max num of tries reached!");
                PlayerLose();
            }
        }
        // Time condition checked on Update
    }

    private void PlayerWin()
    {
        TriggerEnd(true);
    }

    private void PlayerLose()
    {
        TriggerEnd(false);
    }

    private void GameOver()
    {
        gameOver = true;
    }

    private void TriggerEnd(bool isPlayerWinner)
    {
        GameOver();
        if (OnGameOver != null)
        {
            OnGameOver(isPlayerWinner);
        }
    }
}
