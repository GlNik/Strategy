using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBuilding : MonoBehaviour
{

    [SerializeField] private Building _building;
    [SerializeField] Slider _slider;
    [SerializeField] Text _textHp;

    private void Awake()
    {
       // _building.UpdateUIHealth(_building.CheckHp());
        _building.OnChangeHealth.AddListener(SetHealth);
    }

    private void Start()
    {        
        _textHp.text = _building.CheckHp() + " / " + _building.CheckHp();
    }

    public void SetHealth(int health, int maxHealth)
    {
        _slider.value = (float)health / maxHealth;
        _textHp.text = health + " / " + maxHealth;       
    }


}
