using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;

    public static BuildingPlacer Instance;

    [SerializeField] Camera _raycastCamera;
    private Plane _plane;
    public Building CurrentBuilding;

    public Dictionary<Vector2Int, Building> BuildingsDictionary = new Dictionary<Vector2Int, Building>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }
    private void Update()
    {
        if (CurrentBuilding == null) return;

        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;

        if (CheckAllow(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAcceptablePosition();

            if (Input.GetMouseButtonDown(0))
            {
                InstallBuilding(x, z, CurrentBuilding);
                CurrentBuilding = null;
            }
        }
        else
        {
            CurrentBuilding.DisplayUnacceptablePosition();
        }
    }

    bool CheckAllow(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (BuildingsDictionary.ContainsKey(coordinate))
                {
                    return false;
                }
            }
        }
        return true;
    }

    void InstallBuilding(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                BuildingsDictionary.Add(coordinate, CurrentBuilding);
                CurrentBuilding.Place();
            }
        }
    }

    public void CreateBuilding(GameObject buildingPrefab)
    {
        GameObject newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding.GetComponent<Building>();
    }
}
