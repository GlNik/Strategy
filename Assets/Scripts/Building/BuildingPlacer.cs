using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;

    [SerializeField] Camera _raycastCamera;
    private Plane _plane;

    public Building CurrentBuilding;
    public GameObject ReadyBuilding;

    public Dictionary<Vector2Int, Building> BuildingsDictionary = new Dictionary<Vector2Int, Building>();

    //[SerializeField] Management _management;

    Resources _resources;

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

    void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
        _resources = Resources.Instance;
    }


    void Update()
    {
        if (CurrentBuilding == null) return;

        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x) - (CurrentBuilding.XSize / 2 - 1);
        int z = Mathf.RoundToInt(point.z) - (CurrentBuilding.ZSize / 2 - 1);
        x = Mathf.Clamp(x, -60, 60);//-100//92
        z = Mathf.Clamp(z, -38, 45);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;

        if (CheckPlacing(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                int price = CurrentBuilding.GetComponent<Building>().Price;
                _resources.SpendMoney(price);
                //
                var placedBuilding = Instantiate(ReadyBuilding, CurrentBuilding.transform.position, Quaternion.identity);
                placedBuilding.GetComponent<Building>().BuildingIsPlaced = true;
                PlaceBuilding(x, z, placedBuilding.GetComponent<Building>());
                Destroy(CurrentBuilding.gameObject);
                //
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
    public void CreateBuilding(GameObject buildingPrefab)
    {
        if (CurrentBuilding)
            Destroy(CurrentBuilding.gameObject);
        //
        Management.Instance.UnselectAll();
        GameObject newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding.GetComponent<Building>();
        ReadyBuilding = buildingPrefab;
        CurrentBuilding.NavMeshObstacle.enabled = false;
    }

    void PlaceBuilding(int xPos, int zPos, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coords = new Vector2Int(xPos + x, zPos + z);
                BuildingsDictionary.Add(coords, building);
            }
        Debug.Log("есть");
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

    bool CheckPlacing(int xPos, int zPos, Building building)
    {
        try
        {
            // как убрать GetComponentInChildren ?
            if (building.GetComponentInChildren<SelectableCollider>().IntersectingWithSth)
                return false;
        }
        catch
        {
            Debug.LogError("SelectableCollider не найден, может стоит иначе обрабатывать ошибку?");
        }
        // интересно, вместе с проверкой коллайдера выше получаетс€ двойна€ проверка допустимости позиции, больше надЄжности. ѕусть будет.
        for (int x = 0; x < building.XSize; x++)
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coords = new Vector2Int(xPos + x, zPos + z);
                if (BuildingsDictionary.ContainsKey(coords))
                    return false;
            }
        return true;
    }

}
