using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMainBuilding : MonoBehaviour
{
    [SerializeField] Building _mainBuilding;
    void Start()
    {
        Instantiate(_mainBuilding, transform.position, Quaternion.identity);
        //BuildingPlacer.Instance.PlaceBuilding(BuildingPlacer.Instance.X, BuildingPlacer.Instance.Z, _mainBuilding);
    }
}
