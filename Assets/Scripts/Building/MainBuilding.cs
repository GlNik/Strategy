using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : Building
{   
   // private float _startTime;
   // [SerializeField] private float _period = 10f;
    [SerializeField] private  int _workers;
    [SerializeField] private int _maxWorkers;
    [SerializeField] private Unit _workerPrefab;

    //Coroutine _activeCoroutine;

    //public static MainBuilding Instance;// { get; private set; }

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Start()
    {
        WinManager.Instance.AddOurBuilding(this);
    }

    private void Update()
    {
        if (BuildingIsPlaced)
        {
            if (_workers < _maxWorkers)
            {
                PlusOneWorker();
                //StartCoroutinePlusWorker();               
            }
        }
    }

    //private void StartCoroutinePlusWorker()
    //{
    //    if (_activeCoroutine != null)
    //        StopCoroutine(_activeCoroutine);

    //    _activeCoroutine = StartCoroutine(PlusWorker());
    //}

    private void PlusOneWorker()
    {
        _workers++;
        Instantiate(_workerPrefab, SpawnPoint.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(0, 180, 0));
    }

    //private IEnumerator PlusWorker()
    //{                 
    //        _workers++;
    //        Instantiate(_workerPrefab, SpawnPoint.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(0, 180, 0));
    //        yield return null;          
    //}

    public void AddHousingForWorker()
    {
        _maxWorkers+=2;
    }

    //private void OnDestroy()
    //{
    //    if (Instance == this)
    //    {
    //        Instance = null;
    //    }
    //   // WinManager.Instance.RemoveOutBuilding(this);
    //}
}
