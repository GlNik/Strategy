using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public List<EnemyBuilding> _enemyBuildings  = new List<EnemyBuilding>();


    private List<Building> _ourBarraks;
    public static WinManager Instance;

    [SerializeField] private GameObject _winPannel;
    [SerializeField] private GameObject _losePannel;

    public void AddEnemy(EnemyBuilding enemyBuilding)
    {
        _enemyBuildings.Add(enemyBuilding);
    }

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
        _ourBarraks = new List<Building>();        
        _winPannel.SetActive(false);
        _losePannel.SetActive(false);
    }

    public void AddOurBuilding(Building building)
    {
        _ourBarraks.Add(building);
    }

    public void RemoveOutBuilding(Building building)
    {
        _ourBarraks.Remove(building);

        if (_ourBarraks.Count == 0)
        {
            _losePannel.SetActive(true);
        }
    }

    public void AddEnemyBuilding(EnemyBuilding building)
    {
        _enemyBuildings.Add(building);
    }

    public void RemoveEnemyBuilding(EnemyBuilding building)
    {
        _enemyBuildings.Remove(building);

        if (_enemyBuildings.Count == 0)
            _winPannel.SetActive(true);
    }
}
