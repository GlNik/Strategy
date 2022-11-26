using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Barracks : Building
{
    [SerializeField] private Unit _unitPrefab;
    private Resources _resources;
    [SerializeField] private Image _alertNotEnoughMoney;
    private Coroutine _activeCoroutine;

    private void Start()
    {
        _resources = Resources.Instance;

       // WinManager.Instance.AddOurBuilding(this);
    }

    public void TryHire()
    {
        if (_resources.SpendMoney(_unitPrefab.Price))
            Instantiate(_unitPrefab, SpawnPoint.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(0, 180, 0));
        else
            StartCoroutineAlertNotEnoughMoney();
    }

    public void StartCoroutineAlertNotEnoughMoney()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }
        _activeCoroutine = StartCoroutine(ShowAlertNotEnoughMoney());
    }

    public IEnumerator ShowAlertNotEnoughMoney()
    {
        _alertNotEnoughMoney.gameObject.SetActive(true);

        Text childText = _alertNotEnoughMoney.transform.GetChild(0).GetComponent<Text>();

        for (float t = 2f; t > 0f; t -= Time.deltaTime * 1f)
        {
            _alertNotEnoughMoney.color = new Color(1f, 0f, 0f, Mathf.Clamp01(t) * 0.5f);
            childText.color = new Color(1f, 1f, 0f, Mathf.Clamp01(t));
            yield return null;
        }
        _alertNotEnoughMoney.gameObject.SetActive(false);
    }

    //private void OnDestroy()
    //{
    //    WinManager.Instance.RemoveOutBuilding(this);
    //}
}
