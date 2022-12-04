using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Barracks : Building
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Image _alertNotEnoughMoney;
    private Resources _resources;
    private Coroutine _activeCoroutine;

    public override void Start()
    {
        base.Start();
        _resources = Resources.Instance;
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

        float fadeTime = 2f;
        for (float t = 1f; t > 0f; t -= Time.deltaTime / fadeTime)
        {
            _alertNotEnoughMoney.color = new Color(1f, 0f, 0f, t * 0.5f);
            childText.color = new Color(1f, 1f, 0f, t);
            yield return null;
        }
        _alertNotEnoughMoney.gameObject.SetActive(false);
    }
}
