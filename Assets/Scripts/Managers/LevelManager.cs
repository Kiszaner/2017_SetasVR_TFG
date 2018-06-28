using UBUSetasVR.EditorScripts;
using UBUSetasVR.Placement;
using UnityEngine;

namespace UBUSetasVR.Managers
{
    /// <summary>
    /// Class that defines a manager to handle all level related things, like rules and win/lose conditions.
    /// </summary>
    [RequireComponent(typeof(CheckPlacementSet))]
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Is the level environment created at the beginning of the level?
        /// </summary>
        [Tooltip("Is the level environment created at the beginning of the level?")]
        public bool autoPlaceEnvironment = false;

        /// <summary>
        /// Target score for the level.
        /// </summary>
        [Tooltip("Target score for the level")]
        public int targetScore = 10;

        /// <summary>
        /// Time limit for complete the level.
        /// </summary>
        [Tooltip("Time limit for complete the level")]
        public float timeLimit = 120f;

        /// <summary>
        /// Number of successful picked mushrooms needed to finish the level.
        /// </summary>
        [Tooltip("Number of successful picked mushrooms needed to finish the level")]
        public int targetNumSuccess = 3;

        /// <summary>
        /// Number of picked mushrooms needed to finish the level.
        /// </summary>
        [Tooltip("Number of picked mushrooms needed to finish the level")]
        public int targetNumMushrooms = 4;

        /// <summary>
        /// Maximum number of tries for picking mushrooms before level ends.
        /// </summary>
        [Tooltip("Maximum number of tries for picking mushrooms before level ends")]
        public int maxNumTries = 6;

        /// <summary>
        /// Is target score rule enabled?
        /// </summary>
        [Tooltip("Is target score rule enabled?")]
        public bool targetScoreEnabled = true;

        /// <summary>
        /// Is time limit rule enabled?
        /// </summary>
        [Tooltip("Is time limit rule enabled?")]
        public bool timeLimitEnabled = true;

        /// <summary>
        /// Is number of success rule enabled?
        /// </summary>
        [Tooltip("Is number of success rule enabled?")]
        public bool numSuccessEnabled = true;

        //private bool numMushroomsEnable = true;

        /// <summary>
        /// Is maximum number of tries rule enabled?
        /// </summary>
        [Tooltip("Is maximum number of tries rule enabled?")]
        public bool maxTriesEnabled = true;

        /// <summary>
        /// Delegate to handle score updates.
        /// </summary>
        /// <param name="value">The score value updated</param>
        public delegate void ScoreUpdateDelegate(int value);

        /// <summary>
        /// Event fired after score has been updated.
        /// </summary>
        public static event ScoreUpdateDelegate OnScoreUpdated;

        /// <summary>
        /// Delegate to handle a game over situation.
        /// </summary>
        /// <param name="isPlayerWinner">Is the player the winner?</param>
        /// <param name="endGameText">String to display when the game is over</param>
        /// <param name="score">Current score of the player</param>
        public delegate void GameOverDelegate(bool isPlayerWinner, string endGameText, int score);

        /// <summary>
        /// Event fired after game is over.
        /// </summary>
        public static event GameOverDelegate OnGameOver;

        /// <summary>
        /// Delegate that handles the update of the level objectives
        /// </summary>
        /// <param name="score">Is score rule enabled?</param>
        /// <param name="objectiveScore">Target score</param>
        /// <param name="success">Is success rule enabled?</param>
        /// <param name="objectiveSuccess">Target number of pick success</param>
        /// <param name="time">Is time limit rule enabled?</param>
        /// <param name="objectiveTime">Time limit before level ends</param>
        public delegate void ObjectivesUpdateDelegate(bool score, float objectiveScore, bool success, int objectiveSuccess, bool time, float objectiveTime);

        /// <summary>
        /// Event fired after the objectives have been updated.
        /// </summary>
        public static event ObjectivesUpdateDelegate OnObjectivesUpdate;

        /// <summary>
        /// Environmental objects placer.
        /// </summary>
        private BasePlacement placer;

        /// <summary>
        /// Current score of the player.
        /// </summary>
        [SerializeField]
        private int currentScore = 0;

        /// <summary>
        /// Time left before level ends.
        /// </summary>
        private float timeLeft;

        /// <summary>
        /// Current number of successful mushroom picks.
        /// </summary>
        private int currentNumSuccess = 0;

        /// <summary>
        /// Current number of mushrooms picks.
        /// </summary>
        private int currentNumMushrooms = 0;

        /// <summary>
        /// Number of tries left before level ends.
        /// </summary>
        private int triesLeft;

        /// <summary>
        /// Is game over?
        /// </summary>
        private bool gameOver = false;

        /// <summary>
        /// String for the game over screen.
        /// </summary>
        private string endGameText;

        /// <summary>
        /// Unity method that runs everytime the GameObject is enabled in the inspector.
        /// </summary>
        private void OnEnable()
        {
            Basket.OnMushroomInBasket += MushroomInBasket;
        }

        /// <summary>
        /// Unity method that runs everytime the GameObject is disabled in the inspector.
        /// </summary>
        private void OnDisable()
        {
            Basket.OnMushroomInBasket -= MushroomInBasket;
        }

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
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

        /// <summary>
        /// Unity method that runs every frame.
        /// </summary>
        void Update()
        {
            if (gameOver) return;
            if (timeLimitEnabled)
            {
                timeLeft -= Time.deltaTime;
                CheckTimeCondition();
            }
        }

        /// <summary>
        /// Checks if time conditions are met.
        /// </summary>
        private void CheckTimeCondition()
        {
            if (timeLeft <= 0)
            {
                Debug.Log("Time's up!");
                endGameText = "Se ha terminado el tiempo";
                PlayerLose();
            }
        }

        /// <summary>
        /// Handles a mushroom in basket event. It updates internal score if needed with mushroom's value.
        /// </summary>
        /// <param name="mushroom">Mushroom placed in the basket</param>
        /// <param name="success">Is the mushroom in the correct basket?</param>
        /// <param name="value">Value of the mushroom</param>
        /// <param name="infoAlreadyConsulted">Is the mushroom information already consulted?</param>
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

        /// <summary>
        /// Updates the internal variables based on mushroom's values and if it is in the correct basket or not. After that, destroys the mushroom object.
        /// </summary>
        /// <param name="mushroom">Mushroom to get values from</param>
        /// <param name="success">Is in the correct basket?</param>
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

        /// <summary>
        /// Increments or decrements the current score based on the value.
        /// </summary>
        /// <param name="success">Is the choose correct?</param>
        /// <param name="value">Value to update</param>
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

        /// <summary>
        /// Resets the current score to 0.
        /// </summary>
        private void ResetScore()
        {
            Debug.Log("ResetScore");
            currentScore = 0;
            UpdateScoreCounter();
        }

        /// <summary>
        /// Fires an update score event to update with the current score where it's needed.
        /// </summary>
        private void UpdateScoreCounter()
        {
            Debug.Log("UpdateScoreCounter");
            if (OnScoreUpdated != null)
            {
                OnScoreUpdated(currentScore);
            }
        }

        /// <summary>
        /// Checks if any win condition is met or not.
        /// </summary>
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

        /// <summary>
        /// Check if any lose condition is met or not. 
        /// </summary>
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

        /// <summary>
        /// Prepares the winner string and triggers the end game process.
        /// </summary>
        private void PlayerWin()
        {
            Debug.Log("PlayerWin");
            endGameText = endGameText + ", has ganado!";
            TriggerEnd(true);
        }

        /// <summary>
        /// Prepares the loser string and triggers the end game process.
        /// </summary>
        private void PlayerLose()
        {
            Debug.Log("PlayerLose");
            endGameText = endGameText + ", has perdido!";
            TriggerEnd(false);
        }

        /// <summary>
        /// Sets the game to over.
        /// </summary>
        private void GameOver()
        {
            Debug.Log("GameOver");
            gameOver = true;
        }

        /// <summary>
        /// Triggers the end of the level and fires the game over event.
        /// </summary>
        /// <param name="isPlayerWinner">Is the player the winner?</param>
        private void TriggerEnd(bool isPlayerWinner)
        {
            GameOver();
            if (OnGameOver != null)
            {
                OnGameOver(isPlayerWinner, endGameText, currentScore);
            }
        }
    }

    /// <summary>
    /// Enumeration to keep placement types easier.
    /// </summary>
    public enum PlacementType { Random, Weighted }
}
