﻿using UBUSetasVR.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR.UI
{
    public class LevelScore : MonoBehaviour
    {
        public Text ScoreText;

        private void OnEnable()
        {
            LevelManager.OnScoreUpdated += WriteScore;
        }

        private void OnDisable()
        {
            LevelManager.OnScoreUpdated -= WriteScore;
        }

        private void Start()
        {
            WriteScore(0);
        }

        private void WriteScore(int value)
        {
            ScoreText.text = "Puntuación: " + value;
        }
    }
}