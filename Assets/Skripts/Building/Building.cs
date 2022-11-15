using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    public int Price;
    public int XSize = 3;
    public int ZSize = 3;

    private Color _startColor;
    [SerializeField] Renderer[] _renderers;
    [SerializeField] GameObject _menuObject;

    public override void Start()
    {
        base.Start();
        _menuObject.SetActive(false);
    }
    private void Awake()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _startColor = _renderers[i].material.color;
        }      
    }

    // float cellSize = BuildingPlacer.Instance.CellSize;
    private void OnDrawGizmos()
    {
        //float cellSize = BuildingPlacer.Instance.CellSize;
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;
        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1, 0f, 1) * cellSize);

            }
        }
    }

    public override void Select()
    {
        base.Select();
        _menuObject.SetActive(true);
    }
    public override void UnSelect()
    {
        base.UnSelect();
        _menuObject.SetActive(false);
    }

    public void DisplayUnacceptablePosition()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color= Color.red;
        }
       
    }
    public void DisplayAcceptablePosition()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = _startColor;
        }        
    }
}