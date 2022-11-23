using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBuilding : MonoBehaviour
{

    [SerializeField] private Building _building;
    [SerializeField] Slider _slider;

    private void Awake()
    {
        _building.OnChangeHealth.AddListener(SetHealth);
    }

    public void SetHealth(int health, int maxHealth)
    {
        _slider.value = (float)health / maxHealth;
    }

}
