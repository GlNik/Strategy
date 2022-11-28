using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private Transform _spawn;
    [SerializeField] private Enemy _enemyPrefab;
    private float _spawnPeriod;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        _spawnPeriod = Random.Range(20f, 35f);
        if (_timer > _spawnPeriod)
        {
            _timer = 0;
            Instantiate(_enemyPrefab, _spawn.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), _spawn.rotation);
        }
    }
}
