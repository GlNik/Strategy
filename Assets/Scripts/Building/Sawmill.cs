using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sawmill : Building
{
    private Resources _resources;
    private float _startTime;
    private Coroutine _activeCoroutine;

    [SerializeField] private float _period = 10f;
    [SerializeField] private int _woodToAdd = 5;
    [SerializeField] private Text _plusText;

   // public int CounterOfWorkers = 0;

    public override void Start()
    {
        base.Start();
        _resources = Resources.Instance;
        _startTime = Time.time;
    }

    private void Update()
    {
       // if (BuildingIsPlaced)
        {
            if (CounterOfWorkers == 1)
            {
                _plusText.text = "+" + _woodToAdd;

                if (Time.time - _startTime > _period)
                {
                    _resources.AddWood(_woodToAdd);
                    StartCoroutinePlusMoney();
                    _startTime = Time.time;
                }
            }
            else if (CounterOfWorkers == 2)
            {
                _plusText.text = "+" + _woodToAdd * 2;

                if (Time.time - _startTime > _period)
                {
                    _resources.AddWood(_woodToAdd * 2);
                    StartCoroutinePlusMoney();
                    _startTime = Time.time;
                }
            }
        }
    }

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
