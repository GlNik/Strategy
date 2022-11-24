using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class WinManager : MonoBehaviour
{
    public List<EnemyBuilding> _enemyBuildings = new List<EnemyBuilding>();
    public List<Building> OurBarraks;
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
        OurBarraks = new List<Building>();
        _winPannel.SetActive(false);
        _losePannel.SetActive(false);
    }

    public void AddOurBuilding(Building building)
    {
        OurBarraks.Add(building);
    }

    public void RemoveOutBuilding(Building building)
    {
        OurBarraks.Remove(building);

        if (OurBarraks.Count == 0)
        {
            if (_losePannel != null)
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
        {
            if (_losePannel != null)
                _winPannel.SetActive(true);
        }
    }
}
