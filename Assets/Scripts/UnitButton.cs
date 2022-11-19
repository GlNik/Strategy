using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] private Transform _buidingPosition;
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Text _priceText;

    private void Start()
    {
        int price = _unitPrefab.Price;
        _priceText.text = "÷ена:" + price;
    }
    public void TryHire()
    {
        // в Barracks.cs
    }
}
