using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipUGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject _toolTip;   

    private void Awake()
    {
       
        _toolTip.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTip.SetActive(true);       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _toolTip.SetActive(false);       
    }

        
}