using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public BuildingPlacer BuildingPlacer;
    [SerializeField] private Building _buildingPrefab;
    [SerializeField] private Text _priceText;
    private int _price;
    [SerializeField] private Image _alertNotEnoughMoney;
    private Resources _resources;
    private Coroutine _activeCoroutine;

    // ���� ���������� ������ � �� ������� �����, �� ���� �� ��������� ��������� �������� �
    // ������������� ����� ��������� (��������, �������, ������)
    // �� �������� � "������������ �����" ���������, ���� ����� �� ��������� ��������
    // ��� ��� ���������?

    private void Start()
    {
        _resources = Resources.Instance;       
        _price = _buildingPrefab.Price;
        _priceText.text = "����: " + _price;
    }

    public void TryBuy()
    {
        // ��������� ������ ��� ��������� ������!
        if (_resources.CheckMoney(_price))
        {
            BuildingPlacer.CreateBuilding(_buildingPrefab);
        }
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


    private void OnDisable()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }
    }
    
}
