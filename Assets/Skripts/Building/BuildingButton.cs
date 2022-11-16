using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    // [SerializeField] BuildingPlacer _buildingPlacer;
    [SerializeField] GameObject _buildingPrefab;

    public void TryBuy()
    {
        int price = _buildingPrefab.GetComponent<Building>().Price;
        if (Resources.Instance.Money >= price)
        {
            Resources.Instance.Money -= price;
            BuildingPlacer.Instance.CreateBuilding(_buildingPrefab);
        }
        else
        {
            Debug.Log("Нужно больше золота!");
        }
    }

}
