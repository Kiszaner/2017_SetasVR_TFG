using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketsManager : MonoBehaviour
{
    public GameObject BasketPrefab;
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
        for ( int i = 0; i < BasketsPositions.Length; i++)
        {
            pos = new Vector3(BasketsPositions[i].position.x, BasketsPositions[i].position.y - transform.position.y, BasketsPositions[i].position.z);
            Baskets[i] = Instantiate(BasketPrefab, pos, Quaternion.identity, null);
            if (i == 0)
            {
                Baskets[i].GetComponent<Basket>().IsGoodBasket = true;
            }
            Baskets[i].SetActive(false);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        //Debug in Editor Only
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnInputModeChanged();
        }
#endif
    }

    private void OnInputModeChanged()
    {
        bool manipulationInputMode = InputModeManager.currentInputMode == InputModeManager.InputMode.MANIPULATION;
        Vector3 pos;
        for(int i = 0; i < Baskets.Length; i++)
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
        if (Physics.Raycast(defaultPosition, Vector3.down, out hit, DistanceToFloor, FloorMask))
        {
            Debug.Log("Hit");
            targetLocation = hit.point;
            while (CheckObjectsAround(targetLocation, checkObjectsRadius))
            {
                targetLocation = PickRandomPosAroundPoint(hit.point, pickPointRadius);
            }
            //targetLocation += new Vector3(0, transform.localScale.y / 2, 0);
            transform.position = targetLocation;
        }
        else
        {
            Debug.Log("NotHit");
            targetLocation = new Vector3(defaultPosition.x, 0f, defaultPosition.z);
        }

        return targetLocation;
    }

    private bool CheckObjectsAround(Vector3 hitPoint, float radius)
    {
        Debug.Log("CheckAround");
        Collider[] colliders;
        colliders = Physics.OverlapSphere(hitPoint, radius, EnvironmentMask);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    private Vector3 PickRandomPosAroundPoint(Vector3 point, float radius)
    {
        Debug.Log("Picking new random pos");
        Vector2 flatPos = Random.insideUnitCircle * radius;
        Vector3 pos = new Vector3(point.x + flatPos.x, point.y, point.z + flatPos.y);
        Debug.Log("RandomPos: "+pos);
        return pos;
    }
}
