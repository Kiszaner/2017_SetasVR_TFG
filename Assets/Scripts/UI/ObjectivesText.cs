using System;
using UBUSetasVR.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR.UI
{
    /// <summary>
    /// Class that defines the text of the objectives.
    /// </summary>
    public class ObjectivesText : MonoBehaviour
    {
        /// <summary>
        /// Reference to the objectives panel.
        /// </summary>
        public GameObject objectivesPanel;

        /// <summary>
        /// Text of the objectives.
        /// </summary>
        public Text objectivesText;

        /// <summary>
        /// String to be shown when there is a score objective.
        /// </summary>
        public string scoreObjectiveString = "Alcanzar {0} de puntuación.";

        /// <summary>
        /// String to be shown when there is a success objective.
        /// </summary>
        public string successObjectiveString = "Decidir bien para {0} setas.";

        /// <summary>
        /// String to be shown when there is a time limit.
        /// </summary>
        public string timeObjectiveString = "Hay {0} de tiempo límite.";

        /// <summary>
        /// Unity method that runs everytime the GameObject is enabled in the inspector.
        /// </summary>
        private void OnEnable()
        {
            LevelManager.OnObjectivesUpdate += UpdateObjectives;
        }

        /// <summary>
        /// Unity method that runs everytime the GameObject is disabled in the inspector.
        /// </summary>
        private void OnDisable()
        {
            LevelManager.OnObjectivesUpdate -= UpdateObjectives;
        }

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
        void Start()
        {
            objectivesPanel.SetActive(false);
        }

        /// <summary>
        /// Updates the objectives texts.
        /// </summary>
        /// <param name="score">Is score rule enabled?</param>
        /// <param name="objectiveScore">Target score</param>
        /// <param name="success">Is success rule enabled?</param>
        /// <param name="objectiveSuccess">Target number of success</param>
        /// <param name="time">Is time limit rule enabled?</param>
        /// <param name="objectiveTime">Time limit</param>
        private void UpdateObjectives(bool score, float objectiveScore, bool success, int objectiveSuccess, bool time, float objectiveTime)
        {
            if (!objectivesPanel.activeSelf)
            {
                objectivesPanel.SetActive(true);
            }
            string text = "";
            if (score)
            {
                text += string.Format(scoreObjectiveString, objectiveScore);
            }
            if (success)
            {
                text += string.Format("\n\n" + successObjectiveString, objectiveSuccess);
            }
            if (time)
            {
                text += string.Format("\n\n" + timeObjectiveString, TimeFormat(objectiveTime));
            }
            objectivesText.text = text;
        }

        /// <summary>
        /// Converts seconds to a string with mm:ss format.
        /// </summary>
        /// <param name="seconds">Number of seconds to convert</param>
        /// <returns>The string of the time converted</returns>
        private string TimeFormat(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}m:{1:D2}s",
                time.Minutes,
                time.Seconds);
        }
    }
}