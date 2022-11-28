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
        _priceText.text = "Нанять\r\n за: " + price + " золотых";
    }
    public void TryHire()
    {
        // в Barracks.cs
    }
}
