using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    private List<EnemyBuilding> _enemyBuildings = new List<EnemyBuilding>();
    private List<Building> _ourBarraks = new List<Building>();

    public static WinManager Instance;

    [SerializeField] private GameObject _winPannel;
    [SerializeField] private GameObject _losePannel;

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
        // чет ругается, почему? то ругается, то не ругается, хотя вроде норм все?
        if (_enemyBuildings.Count == 0)
        {
            _winPannel.SetActive(true);
        }
    }

}
