using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public static Resources Instance { get; private set; }

    [SerializeField] private int _money;
    [SerializeField] private Text _textMoney;

    [SerializeField] private int _wood;
    [SerializeField] private Text _textWood;

    [SerializeField] private int _food;
    [SerializeField] private Text _textFood;

    [SerializeField] private Color _initialColorMoney;
    [SerializeField] private Color _initialColorWood;
    [SerializeField] private Color _initialColorFood;

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
        _initialColorMoney = _textMoney.color;
        _initialColorWood = _textWood.color;
        _initialColorFood = _textFood.color;

        UpdateText();
    }

    private void UpdateText()
    {
        _textMoney.text = _money.ToString();
        _textWood.text = _wood.ToString();
        _textFood.text = _food.ToString();

        if (_money <= 0) _textMoney.color = new Color(1, 0, 0);
        else _textMoney.color = _initialColorMoney;

        if (_wood <= 0) _textWood.color = new Color(1, 0, 0);
        else _textWood.color = _initialColorWood;

        if (_food <= 0) _textFood.color = new Color(1, 0, 0);
        else _textFood.color = _initialColorFood;
    }

    public bool CheckMoney(int value)
    {
        return _money - value >= 0;
    }

    public bool CheckWood(int value)
    {
        return _wood - value >= 0;
    }

    public bool CheckFood(int value)
    {
        return _food - value >= 0;
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

    public bool SpendWood(int value)
    {
        if (CheckWood(value))
        {
            _wood -= value;
            UpdateText();
            return true;
        }
        return false;
    }

    public bool SpendFood(int value)
    {
        if (CheckFood(value))
        {
            _food -= value;
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

    public void AddWood(int value)
    {
        _wood += value;
        UpdateText();
    }

    public void AddFood(int value)
    {
        _food += value;
        UpdateText();
    }
}
