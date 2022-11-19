using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : Building
{
    private Resources _resources;
    private float _startTime;
    [SerializeField] private float _period = 10f;
    [SerializeField] private int _moneyToAdd = 10;
    [SerializeField] private Text _plusText;

    private void Start()
    {
        _resources = Resources.Instance;
        _startTime = Time.time;
        _plusText.text = "+" + _moneyToAdd.ToString();
    }

    private void Update()
    {
        if (BuildingIsPlaced) // так совпало, что "проблемы первой секунды" не возникает, так что пусть код остаётся в таком виде
        {
            if (Time.time - _startTime > _period)
            {
                _resources.AddMoney(_moneyToAdd);
                StartCoroutinePlusMoney();
                _startTime = Time.time;
            }
        }

    }

    Coroutine _activeCoroutine;
    public void StartCoroutinePlusMoney()
    {
        if (_activeCoroutine != null)
            StopCoroutine(_activeCoroutine);

        _activeCoroutine = StartCoroutine(PlusMoneyEffect());
    }

    public IEnumerator PlusMoneyEffect()
    {
        _plusText.gameObject.SetActive(true);

        _plusText.transform.localPosition = Vector3.zero;

        for (float t = 2f; t > 0f; t -= Time.deltaTime * 1f)
        {
            _plusText.color = new Color(1f, 0.5f, 0f, Mathf.Clamp01(t));

            _plusText.transform.Translate(Vector3.up * Time.deltaTime);

            yield return null;
        }
        _plusText.gameObject.SetActive(false);
    }
}
