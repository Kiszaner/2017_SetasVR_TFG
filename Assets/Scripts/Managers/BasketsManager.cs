using UnityEngine;

namespace UBUSetasVR.Managers
{
    /// <summary>
    /// Class that defines a manager to handle all the creation and placement of the baskets.
    /// </summary>
    public class BasketsManager : MonoBehaviour
    {
        /// <summary>
        /// Prefab of the Good Basket.
        /// </summary>
        [Tooltip("Prefab of the Good Basket")]
        public GameObject GoodBasketPrefab;

        /// <summary>
        /// Prefab of the Bad Basket.
        /// </summary>
        [Tooltip("Prefab of the Bad Basket")]
        public GameObject BadBasketPrefab;

        /// <summary>
        /// Positions in the level to place the baskets.
        /// </summary>
        [Tooltip("Positions in the level to place the baskets")]
        public Transform[] BasketsPositions;

        /// <summary>
        /// Container of the instantiated baskets.
        /// </summary>
        [Tooltip("Container of the instantiated baskets")]
        public GameObject[] Baskets;

        /// <summary>
        /// Layer mask of the floor.
        /// </summary>
        [Tooltip("Layer mask of the floor")]
        public LayerMask FloorMask;

        /// <summary>
        /// Layer mask of the environment.
        /// </summary>
        [Tooltip("Layer mask of the environment")]
        public LayerMask EnvironmentMask;

        /// <summary>
        /// Radius to check if there are objects nearby.
        /// </summary>
        [Tooltip("Radius to check if there are objects nearby")]
        public float checkObjectsRadius = 1f;

        /// <summary>
        /// Radius to pick a point around a certain point.
        /// </summary>
        [Tooltip("Radius to pick a point around a certain point")]
        public float pickPointRadius = 2f;

        /// <summary>
        /// Unity method that runs everytime the GameObject is enabled in the inspector.
        /// </summary>
        private void OnEnable()
        {
            InputModeManager.OnInputModeChange += OnInputModeChanged;
        }

        /// <summary>
        /// Unity method that runs everytime the GameObject is disabled in the inspector.
        /// </summary>
        private void OnDisable()
        {
            InputModeManager.OnInputModeChange -= OnInputModeChanged;
        }

        /// <summary>
        /// Unity method that runs at the beginning of the execution.
        /// </summary>
        private void Start()
        {
            Baskets = new GameObject[BasketsPositions.Length];
            Vector3 pos;
            for (int i = 0; i < BasketsPositions.Length; i++)
            {
                pos = new Vector3(BasketsPositions[i].position.x, BasketsPositions[i].position.y, BasketsPositions[i].position.z);
                if (i == 0)
                {
                    Baskets[i] = Instantiate(GoodBasketPrefab, pos, Quaternion.identity, null);
                    Baskets[i].GetComponent<Basket>().IsGoodBasket = true;
                }
                else
                {
                    Baskets[i] = Instantiate(BadBasketPrefab, pos, Quaternion.identity, null);
                }
                Baskets[i].SetActive(false);
            }
        }

        /// <summary>
        /// Responds after the input mode has been changed. Updates the position of the baskets in scene.
        /// </summary>
        /// <param name="inputMode">Current input mode</param>
        private void OnInputModeChanged(InputMode inputMode)
        {
            bool manipulationInputMode = InputModeManager.currentInputMode == InputMode.MANIPULATION;
            Vector3 pos;
            for (int i = 0; i < Baskets.Length; i++)
            {
                if (manipulationInputMode)
                {
                    pos = CalculateNewBasketPosition(BasketsPositions[i].position);
                    Baskets[i].transform.localPosition = pos;
                }
                Baskets[i].SetActive(manipulationInputMode);
            }
        }

        /// <summary>
        /// Calculates a position in the scene for a new basket raycasting to the floor from an initial posible position.
        /// Takes care of not being too close of an environment object.
        /// </summary>
        /// <param name="defaultPosition">A posible initial position</param>
        /// <returns>The calculated position</returns>
        private Vector3 CalculateNewBasketPosition(Vector3 defaultPosition)
        {
            RaycastHit hit;
            Vector3 targetLocation;
            // Check downwards or upwards to get a position in the floor
            if (Physics.Raycast(defaultPosition, Vector3.down, out hit, Mathf.Infinity, FloorMask) ||
                Physics.Raycast(defaultPosition, Vector3.up, out hit, Mathf.Infinity, FloorMask))
            {
                targetLocation = hit.point;
                // While not getting a position clear of nearby environment objects, keep looking for a position
                while (AuxiliarFunctions.CheckObjectsAround(targetLocation, checkObjectsRadius, EnvironmentMask))
                {
                    targetLocation = AuxiliarFunctions.PickRandomPosAroundPoint(hit.point, pickPointRadius);
                }
            }
            else
            {
                Debug.LogError("NoFloortHit");
                targetLocation = new Vector3(defaultPosition.x, 0f, defaultPosition.z);
            }
            return targetLocation;
        }
    }
}