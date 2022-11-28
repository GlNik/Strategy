using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public static Resources Instance { get; private set; }

    [SerializeField] private int _money;
    [SerializeField] private Text _textMoney;
    private Color _initialColor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        _initialColor = _textMoney.color;
        UpdateText();
    }

    private void UpdateText()
    {
        _textMoney.text = _money.ToString();
        if (_money <= 0) _textMoney.color = new Color(1, 0, 0);
        else _textMoney.color = _initialColor;
    }
    public bool CheckMoney(int value)
    {
        return _money - value >= 0;
    }

    public bool SpendMoney(int value)
    {
        if (CheckMoney(value))
        {
            _money -= value;
            UpdateText();
            return true;
        }
        return false;
    }

    public void AddMoney(int value)
    {
        _money += value;
        UpdateText();
    }
}
