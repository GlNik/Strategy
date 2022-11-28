using UnityEngine;


public class CreateMenuButton : MonoBehaviour
{
    //private bool _isMenuActive;
    [SerializeField] private GameObject _createMenuButton;
    
    public void ShowMenu()
    {
        _createMenuButton.SetActive(!_createMenuButton.activeSelf);        
    }

}
