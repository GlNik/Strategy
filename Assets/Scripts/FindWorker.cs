using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindWorker : MonoBehaviour
{
    private Button _button;
    private bool _freeState;
    private Worker _worker;
    private Vector3 _spawnPoint;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _icon;

    private void Start()
    {
        _freeState = false;
        _worker = null;
        _spawnPoint = GetSpawnPoint.Instance.GetSpawnPointTransform();
        _icon.SetActive(false);
    }

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ActiveSelfWorker);
    }

    public void ActiveSelfWorker()
    {        
        if (!_freeState)
        {
            if (UnitsManager.Instance.FreeWorker.Count <= 0) return;

            _freeState = true;
            _worker = UnitsManager.Instance.GetFreeWorker();
            _worker.MoveToBuilding(_transform.position);
            _icon.SetActive(true);
        }
        else
        {
            _freeState = false;
            //UnitsManager.Instance.SetFreeWorker(_worker);
            _worker.ReturnToSpawnPoint(_spawnPoint);
            _icon.SetActive(false);
        }
    
    }

    private void OnDisable()
    {
        _button?.onClick.RemoveListener(ActiveSelfWorker);
    }
}
