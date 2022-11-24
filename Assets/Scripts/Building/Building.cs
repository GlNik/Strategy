using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Building : SelectableObject
{
    public int Price;
    public int XSize = 3;
    public int ZSize = 3;

    private Color _startColor;
    [SerializeField] private Renderer[] _renderers;
    public Transform SpawnPoint;
    [SerializeField] private int _health;
    private int _maxHealth;
    public NavMeshObstacle NavMeshObstacle;
    [SerializeField] private GameObject _buildingMenu;

    public bool BuildingIsPlaced = false;
    public UnityEvent<int, int> OnChangeHealth;

    private void Awake()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _startColor = _renderers[i].material.color;
        }
        _maxHealth = _health;
        _buildingMenu.SetActive(false);
    }

    public override void Select()
    {
        if (BuildingIsPlaced)
        {
            base.Select();
            _buildingMenu.SetActive(true);
        }
    }
    public override void Unselect()
    {
        base.Unselect();
        try
        {
            _buildingMenu.SetActive(false);
        }
        catch { };
    }

    //private void OnDrawGizmos()
    //{
    //    float cellSize = BuildingPlacer.Instance.CellSize;

    //    for (int x = 0; x < XSize; x++)
    //        for (int z = 0; z < ZSize; z++)
    //        {
    //            Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1, 0, 1) * cellSize);
    //        }
    //}

    private void OnDrawGizmos()
    {
        float cellSize = BuildingPlacer.Instance.CellSize;
        Vector2 offset = new Vector2((XSize - 1f) * 0.5f, (ZSize - 1f) * 0.5f);
        for (int i = 0; i < XSize; i++)
        {
            for (int j = 0; j < ZSize; j++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(i - offset.x, 0f, j - offset.y) * cellSize, new Vector3(1f, 0f, 1f) * cellSize);
            }
        }
    }

    public void DisplayUnacceptablePosition()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = Color.red;
        }
    }

    public void DisplayAcceptablePosition()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = Color.green;
        }
    }

    public void DisplayUsual()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = Color.white;
        }
    }

    public void DestroyBuilding()
    {
        BuildingPlacer.Instance.ReleasePlace(transform.position.x, transform.position.z, this);
        Destroy(gameObject);
    }

    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        if (_health <= 0)
        {
            DestroyBuilding();
        }
        HealthBar.SetHealth(_health, _maxHealth);
        OnChangeHealth.Invoke(_health, _maxHealth);
    }

}
