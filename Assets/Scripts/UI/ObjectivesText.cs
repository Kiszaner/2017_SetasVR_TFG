using System;
using UBUSetasVR.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR.UI
{
    public class ObjectivesText : MonoBehaviour
    {
        public GameObject objectivesPanel;
        public Text objectivesText;
        public string scoreObjectiveString = "Alcanzar {0} de puntuación.";
        public string successObjectiveString = "Decidir bien para {0} setas.";
        public string timeObjectiveString = "Hay {0} de tiempo límite.";

        private void OnEnable()
        {
            LevelManager.OnObjectivesUpdate += UpdateObjectives;
        }

        private void OnDisable()
        {
            LevelManager.OnObjectivesUpdate -= UpdateObjectives;
        }

        // Use this for initialization
        void Start()
        {
            objectivesPanel.SetActive(false);
        }

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

        private string TimeFormat(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}m:{1:D2}s",
                time.Minutes,
                time.Seconds);
        }
    }
}

