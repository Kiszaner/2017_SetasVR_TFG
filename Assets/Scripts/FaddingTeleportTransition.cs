using UnityEngine;
using DaydreamElements.Main;
using DaydreamElements.Teleport;

namespace UBUSetasVR
{
    /// Animate player teleport transition with a linear animation.
    public class FaddingTeleportTransition : BaseTeleportTransition
    {
        /// Speed of Lerp transition.
        [Tooltip("Speed of transition")]
        public float transitionSpeed = 10.0f;
        public ScreenFade fader;

        /// True if transition is in progress.
        public override bool IsTransitioning { get { return isTransitioning; } }

        private bool isTransitioning;
        private Vector3 targetPosition;
        private Transform player;

        void Update()
        {
            if (isTransitioning == false)
            {
                return;
            }

            // Animate player to position with linear steps
            player.position = Vector3.MoveTowards(
              player.position,
              targetPosition,
              transitionSpeed * Time.deltaTime);

            // Check if transition is finished.
            if (player.transform.position.Equals(targetPosition))
            {
                fader.FadeToClear();
                isTransitioning = false;
            }
        }

        public override void StartTransition(
            Transform playerTransform, Transform controller, Vector3 target)
        {
            player = playerTransform;
            targetPosition = target;
            fader.FadeToColor();
            isTransitioning = true;
        }

        public override void CancelTransition()
        {
            fader.FadeToClear();
            isTransitioning = false;
        }
    }
}