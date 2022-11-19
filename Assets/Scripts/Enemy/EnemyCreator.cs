using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private Transform _spawn;
    [SerializeField] private float _spawnPeriod = 5;
    [SerializeField] private Enemy _enemyPrefab;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _spawnPeriod)
        {
            _timer = 0;
            Instantiate(_enemyPrefab, _spawn.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), _spawn.rotation);
        }
    }
}
