using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;
    [SerializeField] private Camera _raycastCamera;
    private Plane _plane;
    public Building CurrentBuilding;
    public Building ReadyBuilding;
    public Dictionary<Vector2Int, Building> BuildingsDictionary = new Dictionary<Vector2Int, Building>();
    private Resources _resources;

    [SerializeField] private Transform _cornerA;
    [SerializeField] private Transform _cornerB;

    private Vector3Int _cornerAPosition;
    private Vector3Int _cornerBPosition;
    private float _elapsedTime;

    public static BuildingPlacer Instance;

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
        _resources = Resources.Instance;
        SetSizeMap();
    }

    private void OnDrawGizmos()
    {
        SetSizeMap();

        Vector3 center = (Vector3)(_cornerAPosition + _cornerBPosition) * CellSize / 2f;
        Vector3 size = (Vector3)(_cornerBPosition - _cornerAPosition) * CellSize;
        size.y = 0f;
        Gizmos.color = Color.black;
        Gizmos.DrawCube(center, size);
    }

    private void SetSizeMap()
    {
        _cornerAPosition = new Vector3Int(Mathf.RoundToInt(_cornerA.position.x / CellSize), 0, Mathf.RoundToInt(_cornerA.position.z / CellSize));
        _cornerBPosition = new Vector3Int(Mathf.RoundToInt(_cornerB.position.x / CellSize), 0, Mathf.RoundToInt(_cornerB.position.z / CellSize));
    }

    private void Update()
    {
        if (CurrentBuilding == null) return;

        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x) - (CurrentBuilding.XSize / 2 - 1);
        int z = Mathf.RoundToInt(point.z) - (CurrentBuilding.ZSize / 2 - 1);

        //x = Mathf.Clamp(x, -60, 60);
        //z = Mathf.Clamp(z, -38, 45);
        x = Mathf.Clamp(x, _cornerAPosition.x, _cornerBPosition.x);
        z = Mathf.Clamp(z, _cornerAPosition.z, _cornerBPosition.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;

        if (CheckPlacing(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                int price = CurrentBuilding.Price;
                _resources.SpendMoney(price);

                var placedBuilding = Instantiate(ReadyBuilding, CurrentBuilding.transform.position, Quaternion.identity);
                placedBuilding.BuildingIsPlaced = true;

                PlaceBuilding(x, z, placedBuilding);

                Destroy(CurrentBuilding.gameObject);

                CurrentBuilding = null;
            }
        }
        else
            CurrentBuilding.DisplayUnacceptablePosition();

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(CurrentBuilding.gameObject);
            CurrentBuilding = null;
        }

        //_elapsedTime += Time.deltaTime;
        //if (Input.GetKey(KeyCode.C) && _elapsedTime > 0.2f)
        //{
        //    RotateBuilding(90f);
        //}
        //if (Input.GetKey(KeyCode.V) && _elapsedTime > 0.2f)
        //{
        //    RotateBuilding(-90f);
        //}

    }

    public Building GetClousestBuilding(Vector3 position)
    {
        Building clousestBuilding = null;
        float minDistance = Mathf.Infinity;
        foreach (var item in BuildingsDictionary)
        {
            float distance = Vector3.Distance(position, item.Value.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                clousestBuilding = item.Value;
            }
        }
        return clousestBuilding;
    }

    public void CreateBuilding(Building buildingPrefab)
    {
        if (CurrentBuilding)
            Destroy(CurrentBuilding.gameObject);
        //
        Management.Instance.UnselectAll();
        Building newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding;
        ReadyBuilding = buildingPrefab;
        CurrentBuilding.NavMeshObstacle.enabled = false;
    }

    private void PlaceBuilding(int xPos, int zPos, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coords = new Vector2Int(xPos + x, zPos + z);
                BuildingsDictionary.Add(coords, building);
            }
    }

    public void ReleasePlace(float xPos, float zPos, Building building)
    {
        int xInt = Mathf.RoundToInt(xPos / CellSize);
        int zInt = Mathf.RoundToInt(zPos / CellSize);

        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coords = new Vector2Int(xInt + x, zInt + z);
                BuildingsDictionary.Remove(coords);
            }
        }
    }

    private bool CheckPlacing(int xPos, int zPos, Building building)
    {
        try
        {
            if (building.GetComponentInChildren<SelectableCollider>().IntersectingWithSth)
                return false;
        }
        catch
        {
            Debug.LogError("SelectableCollider �� ������, ����� ����� ����� ������������ ������?");
        }
        for (int x = 0; x < building.XSize; x++)
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coords = new Vector2Int(xPos + x, zPos + z);
                if (BuildingsDictionary.ContainsKey(coords))
                    return false;
            }
        return true;
    }

    private void RotateBuilding(float degrees)
    {
        _elapsedTime = 0f;

        //Quaternion rotationRightReady = Quaternion.Euler(new Vector3(0f, degrees, 0f)) * ReadyBuilding.transform.rotation;
        Quaternion rotationRight = Quaternion.Euler(new Vector3(0f, degrees, 0f)) * CurrentBuilding.transform.rotation;
        CurrentBuilding.transform.rotation = rotationRight;
        // ReadyBuilding.transform.rotation = rotationRightReady;
    }

}
