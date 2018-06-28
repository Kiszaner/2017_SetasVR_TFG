using UBUSetasVR.UI;
using UnityEngine;

namespace UBUSetasVR
{
    /// <summary>
    /// Class that defines a basket.
    /// </summary>
    public class Basket : MonoBehaviour
    {
        /// <summary>
        /// Is the basket a Good Basket?
        /// </summary>
        public bool IsGoodBasket;

        /// <summary>
        /// Delegate to handle a mushroom in the basket.
        /// </summary>
        /// <param name="mushroom">Mushroom in the basket</param>
        /// <param name="success">Is the correct basket for the mushroom?</param>
        /// <param name="value">Value of the mushroom</param>
        /// <param name="consulted">Is the information of the mushroom already consulted?</param>
        public delegate void MushroomInBasket(GameObject mushroom, bool success, int value, bool consulted);

        /// <summary>
        /// Event that fires after a mushroom is throwed inside a basket.
        /// </summary>
        public static event MushroomInBasket OnMushroomInBasket;

        /// <summary>
        /// Unity method to check physics collisions between objects.
        /// </summary>
        /// <param name="other">Collider from the object collided</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Mushroom"))
            {
                GameObject mushroom = other.gameObject;
                MushroomInfo info = mushroom.GetComponent<MushroomInfo>();
                if (info != null)
                {
                    CheckConditions(info, mushroom);
                }
            }
        }

        /// <summary>
        /// Checks if the mushroom is in the correct basket or not.
        /// </summary>
        /// <param name="info">Information of the mushroom</param>
        /// <param name="mushroom">Mushroom instance</param>
        private void CheckConditions(MushroomInfo info, GameObject mushroom)
        {
            if (info.Mushroom.IsEdible || info.Mushroom.IsRecommended)
            {
                Debug.Log("Comestible o recomendada");
                if (IsGoodBasket)
                {
                    OnMushroomInBasket(mushroom, true, info.Mushroom.ScoreValue, info.infoAlreadyColsulted);
                }
                else
                {
                    OnMushroomInBasket(mushroom, false, info.Mushroom.ScoreValue, info.infoAlreadyColsulted);
                }
            }
            else
            {
                Debug.Log("Otra cosa");
                if (IsGoodBasket)
                {
                    OnMushroomInBasket(mushroom, false, info.Mushroom.ScoreValue, info.infoAlreadyColsulted);
                }
                else
                {
                    OnMushroomInBasket(mushroom, true, info.Mushroom.ScoreValue, info.infoAlreadyColsulted);
                }
            }
        }
    }
}