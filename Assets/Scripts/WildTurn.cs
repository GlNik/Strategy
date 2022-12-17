using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WildTurn : MonoBehaviour
{
    [SerializeField] private float _timer = 90f; //5 мин
    [SerializeField] private Text _textTimer;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _currentEnemyCount = 0;
    [SerializeField] private int _currentTurn;
    [SerializeField] private List<int> _listOfEnemyCount;
    [SerializeField] private Transform _boxGenerationArea;
    [SerializeField] private int _maxWave;

    private void Update()
    {
        _timer -= Time.deltaTime;
        UpdateTurnTime();
        if (_timer <= 0)
        {
            // что бы всегда была последняя волна 
            if (_currentTurn < _maxWave)
            {
                _currentEnemyCount = _listOfEnemyCount[_currentTurn];
                _currentEnemyCount++;
                _currentTurn++;
            }
            for (int i = 1; i < _currentEnemyCount; i++)
            {
                Vector3 newPosition = _boxGenerationArea.TransformPoint(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                Instantiate(_enemyPrefab, newPosition + Vector3.back * Random.Range(-2f, 2f), Quaternion.identity);
            }
            _timer = 300f;
        }
    }

    void UpdateTurnTime()
    {
        _textTimer.text = _timer.ToString("0:00");
    }
}
