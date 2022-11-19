using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenuButton : MonoBehaviour
{
    private bool _isMenuActive;
    [SerializeField] private GameObject _CreateMenuButton;
    
    public void ShowMenu()
    {
        _CreateMenuButton.SetActive(!_CreateMenuButton.activeSelf);        
    }

}
