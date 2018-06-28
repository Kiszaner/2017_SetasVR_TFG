using UnityEngine;

namespace UBUSetasVR
{
    /// <summary>
    /// Class that defines a method to "stare" constantly at the player.
    /// </summary>
    public class LookAtPlayer : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player.
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Unity method that runs every frame.
        /// </summary>
        void Update()
        {
            if (player == null) return;
            transform.LookAt(2 * transform.position - player.transform.position);
        }
    }
}