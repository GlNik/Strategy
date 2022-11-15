using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{

    [SerializeField] GameObject _selectionIndicator;

    public virtual void Start()
    {
        _selectionIndicator.SetActive(false);
    }
    public virtual void OnHover()
    {
        //transform.localScale = Vector3.one * 1.1f;
    }
    public virtual void UnOnHover()
    {
       // transform.localScale = Vector3.one;
    }

    public virtual void Select()
    {
        _selectionIndicator.SetActive(true);
    }

    public virtual void UnSelect()
    {
        _selectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point)
    {

    }
}
