using UBUSetasVR.EditorScripts;
using UBUSetasVR.Placement;
using UnityEngine;

namespace UBUSetasVR.Managers
{
    [RequireComponent(typeof(CheckPlacementSet))]
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
        public bool numSuccessEnabled = true;
        //private bool numMushroomsEnable = true;
        public bool maxTriesEnabled = true;
        public delegate void ScoreUpdateDelegate(int value);
        public static event ScoreUpdateDelegate OnScoreUpdated;
        public delegate void GameOverDelegate(bool isPlayerWinner, string endGameText, int score);
        public static event GameOverDelegate OnGameOver;
        public delegate void ObjectivesUpdateDelegate(bool score, float objectiveScore, bool success, int objectiveSuccess, bool time, float objectiveTime);
        public static event ObjectivesUpdateDelegate OnObjectivesUpdate;

        private BasePlacement placer;
        [SerializeField]
        private int currentScore = 0;
        private float timeLeft;
        private int currentNumSuccess = 0;
        private int currentNumMushrooms = 0;
        private int triesLeft;
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

        void Start()
        {
            placer = GetComponent<BasePlacement>();
            ResetScore();
            timeLeft = timeLimit;
            triesLeft = maxNumTries;
            if (autoPlaceEnvironment)
            {
                placer.RepeatAllPlacement();
            }
            if (OnObjectivesUpdate != null)
            {
                OnObjectivesUpdate(targetScoreEnabled, targetScore, numSuccessEnabled, targetNumSuccess, timeLimitEnabled, timeLimit);
            }
        }

        void Update()
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

        private void MushroomInBasket(GameObject mushroom, bool success, int value, bool infoAlreadyConsulted)
        {
            Debug.Log("MushroomInBasket");
            if (infoAlreadyConsulted || gameOver)
            {
                Debug.Log("Información consultada o fin del juego alcanzado");
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
            placer.DestroyGameObject(mushroom);
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
                if (currentNumSuccess >= targetNumSuccess)
                {
                    Debug.Log("Target num of success reached!");
                    endGameText = "Acertadas las setas necesarias";
                    PlayerWin();
                }
            }
        }

        private void CheckLoseConditions()
        {
            Debug.Log("CheckLoseConditions");
            if (maxTriesEnabled)
            {
                if (triesLeft <= 0)
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
                OnGameOver(isPlayerWinner, endGameText, currentScore);
            }
        }
    }

    public enum PlacementType { Random, Weighted }
}
