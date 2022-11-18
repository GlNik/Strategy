using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
	[SerializeField] Transform _buidingPosition;
	[SerializeField] Unit _unitPrefab;	
	[SerializeField] Text _priceText;

	private void Start()
	{
		int price = _unitPrefab.Price;
		_priceText.text = "����:" + price;
	}
	public void TryHire()
	{
		// � Barracks.cs
	}
}
