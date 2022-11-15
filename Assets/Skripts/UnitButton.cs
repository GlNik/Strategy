using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] GameObject _unitPrefab;
    [SerializeField] Text _priceText;
    [SerializeField] Barrack _barrack;

    private void Start()
    {
        _priceText.text = "Цена: " + _unitPrefab.GetComponent<Unit>().Price;
    }

    public void TryBuy()
    {
        int price = _unitPrefab.GetComponent<Unit>().Price;
        if (Resources.Instance.Money >= price)
        {
            Resources.Instance.Money -= price;
            _barrack.CreateUnit(_unitPrefab);
        }
        else
        {
            Debug.Log("Нужно больше золота!");
        }
    }


}
