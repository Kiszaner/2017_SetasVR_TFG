using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR
{
    public class BasketsManager : MonoBehaviour
    {
        public GameObject GoodBasketPrefab;
        public GameObject BadBasketPrefab;
        public Transform[] BasketsPositions;
        public GameObject[] Baskets;
        public float DistanceToFloor = 2.3f;
        public LayerMask FloorMask;
        public LayerMask EnvironmentMask;

        private float checkObjectsRadius = 1f;
        private float pickPointRadius = 2f;

        private void OnEnable()
        {
            InputModeManager.OnInputModeChange += OnInputModeChanged;
        }

        private void OnDisable()
        {
            InputModeManager.OnInputModeChange -= OnInputModeChanged;
        }

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

        private Vector3 CalculateNewBasketPosition(Vector3 defaultPosition)
        {
            RaycastHit hit;
            Vector3 targetLocation;
            Debug.Log("DefPos: " + defaultPosition);
            if (Physics.Raycast(defaultPosition, Vector3.down, out hit, Mathf.Infinity, FloorMask) ||
                Physics.Raycast(defaultPosition, Vector3.up, out hit, Mathf.Infinity, FloorMask))
            {
                Debug.Log("Hit");
                targetLocation = hit.point;
                Debug.Log("HitPos: " + targetLocation);
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
            Debug.Log("TargPos: " + targetLocation);
            return targetLocation;
        }
    }
}