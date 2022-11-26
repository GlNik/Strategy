using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building
{
    [Space(12)]
    [Header("Main Building")]
    [SerializeField] private int _workers;
    [SerializeField] private int _maxWorkers;
    [SerializeField] private Unit _workerPrefab;

    Coroutine _activeCoroutine;

    [SerializeField] float _timeToBuildUnit = 5f;

    [SerializeField] private Text _progressText;
    [SerializeField] private Slider _sliderProgress;
    [SerializeField] private Image _circleProgressBar;

    private void Start()
    {
        WinManager.Instance.AddOurBuilding(this);

        _progressText.text = "0%";
        _sliderProgress.value = 0;
    }

    private void Update()
    {
        if (BuildingIsPlaced)
        {
            if (_workers < _maxWorkers)
            {
                GeneratingUnit();
                // PlusOneWorker();
                //StartCoroutinePlusWorker();               
            }
        }
    }

    //private void PlusOneWorker()
    //{
    //    _workers++;
    //    Instantiate(_workerPrefab, SpawnPoint.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(0, 180, 0));
    //}   

    public void AddHousingForWorker()
    {
        _maxWorkers += 2;
    }

    public void GeneratingUnit()
    {
        if (_activeCoroutine == null)
        {
            _activeCoroutine = StartCoroutine(UnitCreationProcess());
        }
    }

    private IEnumerator UnitCreationProcess()
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / _timeToBuildUnit)
        {
            _circleProgressBar.fillAmount = t;
            _sliderProgress.value = t;
            _progressText.text = (t * 100).ToString("0") + "%";

            yield return null;
        }
        _workers++;
        Instantiate(_workerPrefab, SpawnPoint.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(0, 180, 0));
        _sliderProgress.value = 0f;
        _circleProgressBar.fillAmount = 0f;
        _progressText.text = "0%";


        _activeCoroutine = null;
    }
}
