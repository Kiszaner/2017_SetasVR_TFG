using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int TargetScore = 10;
    public float timeLimit = 120f;
    public bool timeLimitEnabled = true;
    public bool targetScoreEnabled = true;
    public delegate void ScoreUpdateDelegate(int value);
    public static event ScoreUpdateDelegate OnScoreUpdated;
    public delegate void EndConditionDelegate(bool isPlayerWinner);
    public static event EndConditionDelegate OnEndConditionReached;

    [SerializeField]
    private int CurrentScore = 0;
    private float timeLeft;

    private void OnEnable()
    {
        Basket.OnMushroomInBasket += UpdateScore;
    }

    private void OnDisable()
    {
        Basket.OnMushroomInBasket -= UpdateScore;
    }

    // Use this for initialization
    void Start ()
    {
        ResetScore();
        timeLeft = timeLimit;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (timeLimitEnabled)
        {
            CheckTimeCondition();
        }
    }

    private void CheckTimeCondition()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            GameOver();
        }
    }

    private void UpdateScore(bool success, int value)
    {
        if (success)
        {
            CurrentScore += value;
        }
        else
        {
            CurrentScore -= value;
        }
        UpdateScoreCounter();
        CheckWinCondition();
    }

    private void ResetScore()
    {
        CurrentScore = 0;
        UpdateScoreCounter();
    }

    private void UpdateScoreCounter()
    {
        if (OnScoreUpdated != null)
        {
            OnScoreUpdated(CurrentScore);
        }
    }

    private void CheckWinCondition()
    {
        if(CurrentScore >= TargetScore)
        {
            TriggerEnd(true);
        }
    }

    private void GameOver()
    {
        TriggerEnd(false);
    }

    private void TriggerEnd(bool isPlayerWinner)
    {
        if (OnEndConditionReached != null)
        {
            OnEndConditionReached(isPlayerWinner);
        }
    }
}
