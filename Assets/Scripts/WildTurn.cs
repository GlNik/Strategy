using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WildTurn : MonoBehaviour
{
    float _timer;
    [SerializeField] float _maxTimer;
    [SerializeField] TextMeshProUGUI _textTimeToTurn;
    [SerializeField] List<float> _timers;
    [SerializeField] Enemy _enemyPrefab;
    [SerializeField] int _currentEnemyCount;
    [SerializeField] List<int> _listOfEnemyCount;
    [SerializeField] Transform _boxGenerationArea;
    private void Awake()
    {
        _timer = _maxTimer;
    }
    private void Update()
    {
        _timer -= Time.deltaTime;
        UpdateTurnTime();
        if (_timer < 0)
        {
            Vector3 newPosition = _boxGenerationArea.TransformPoint(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));

            for (int i = 0; i < _currentEnemyCount; i++)
            {
                Enemy newEnemy = Instantiate(_enemyPrefab, newPosition + Vector3.back * Random.Range(-2f, 2f), Quaternion.identity);

            }
        }
    }

    void UpdateTurnTime()
    {
        _textTimeToTurn.text = "<color=#adadad>Next Wild Turn: </color>" + _timer.ToString("0") + " sec";
    }
}
