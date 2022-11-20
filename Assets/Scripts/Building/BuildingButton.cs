using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingButton : MonoBehaviour
{
    public BuildingPlacer BuildingPlacer;
    [SerializeField] private Building _buildingPrefab;
    [SerializeField] private Text _priceText;
    private int _price;
    [SerializeField] private CanvasGroup _alertNotEnoughMoney;
    private Resources _resources;
    private Coroutine _activeCoroutine;

    private Button _buyButton;
    private bool _showState;    

    private void Awake()
    {
        _alertNotEnoughMoney.alpha = 0;
        _buyButton = GetComponent<Button>();
    }

    private void Start()
    {
        _resources = Resources.Instance;
        _price = _buildingPrefab.Price;
        _priceText.text = "Цена: " + _price;
    }
  
    public void TryBuy()
    {
        // списываем только при проставке здания!
        if (_resources.CheckMoney(_price))
        {
            BuildingPlacer.CreateBuilding(_buildingPrefab);
        }
        else
            ShowNoMoneyFrame();
    }

    private void ShowNoMoneyFrame()
    {
        if (!_showState)
        {
            _showState = true;
            Tween tween = _alertNotEnoughMoney.DOFade(1, 0.1f);
            tween = _alertNotEnoughMoney.DOFade(0, 0.5f).SetDelay(1);
            tween.onComplete += () => { _showState = false; };
        }
    }

    private void OnDisable()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }
    }
}
