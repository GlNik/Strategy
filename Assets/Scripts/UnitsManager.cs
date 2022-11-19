using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    private List<Knight> _knight = new List<Knight>();
    private List<Enemy> _enemies = new List<Enemy>();

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
    public void AddKnight(Knight knithg)
    {
        _knight.Add(knithg);
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveKnight(Knight knight)
    {
        _knight.Remove(knight);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
    public Unit GetClousestUnit(Vector3 position)
    {
        Unit clousestUnit = null;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < _knight.Count; i++)
        {
            float distance = Vector3.Distance(position, _knight[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                clousestUnit = _knight[i];
            }
        }
        return clousestUnit;
    }
    public Enemy GetClousestEnemy(Vector3 position, float distanceToEnemy)
    {
        Enemy clousestEnemy = null;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < _enemies.Count; i++)
        {
            float distance = Vector3.Distance(position, _enemies[i].transform.position);
            if (distance < minDistance && distance < distanceToEnemy)
            {
                minDistance = distance;
                clousestEnemy = _enemies[i];
            }
        }
        return clousestEnemy;
    }
}
