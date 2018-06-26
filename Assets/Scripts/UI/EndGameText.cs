using UBUSetasVR.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR.UI
{
    [RequireComponent(typeof(Canvas))]
    public class EndGameText : MonoBehaviour
    {
        public Text endGameText;
        public GameObject endGameTextParent;

        private void OnEnable()
        {
            LevelManager.OnGameOver += OnGameIsOver;
        }

        private void OnDisable()
        {
            LevelManager.OnGameOver -= OnGameIsOver;
        }

        private void OnGameIsOver(bool isPlayerWinner, string endGameMessage, int score)
        {
            if (endGameTextParent != null)
            {
                endGameTextParent.SetActive(true);
            }
            endGameText.gameObject.SetActive(true);
            endGameText.text = endGameMessage + ".\n\n Puntuación alcanzada: " + score; ;
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
}