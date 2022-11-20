using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    private List<Viking> _viking = new List<Viking>();
    private List<Enemy> _enemy = new List<Enemy>();

    public static UnitsManager Instance;

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
    public void AddViking(Viking viking)
    {
        _viking.Add(viking);
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemy.Add(enemy);
    }

    public void RemoveViking(Viking viking)
    {
        _viking.Remove(viking);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemy.Remove(enemy);
    }
    public Unit GetClousestUnit(Vector3 position)
    {
        Unit clousestUnit = null;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < _viking.Count; i++)
        {
            float distance = Vector3.Distance(position, _viking[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                clousestUnit = _viking[i];
            }
        }
        return clousestUnit;
    }
    public Enemy GetClousestEnemy(Vector3 position, float distanceToEnemy)
    {
        Enemy clousestEnemy = null;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < _enemy.Count; i++)
        {
            float distance = Vector3.Distance(position, _enemy[i].transform.position);
            if (distance < minDistance && distance < distanceToEnemy)
            {
                minDistance = distance;
                clousestEnemy = _enemy[i];
            }
        }
        return clousestEnemy;
    }
}
