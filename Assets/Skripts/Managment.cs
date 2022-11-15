using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managment : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] SelectableObject _hovered;
    [SerializeField] List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectableCollider>() is SelectableCollider selectable)
            {
                SelectableObject hitSelectableObject = selectable.SelectableObject;
                if (_hovered)
                {
                    if (_hovered != hitSelectableObject)
                    {
                        _hovered.UnOnHover();
                        _hovered = hitSelectableObject;
                        _hovered.OnHover();
                    }
                }
                else
                {
                    _hovered = hitSelectableObject;
                    _hovered.OnHover();
                }
            }
            else
            {
                UnhoverCurrent();
            }
        }
        else
        {
            UnhoverCurrent();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_hovered)
            {
                if (Input.GetKey(KeyCode.LeftControl) == false)
                {
                    UnSelectAll();
                }
                Select(_hovered);
            }

            if(hit.collider.tag =="Ground")
            {
                for (int i = 0; i < _listOfSelected.Count; i++)
                {
                    _listOfSelected[i].WhenClickOnGround(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            UnSelectAll();
        }
    }

    void Select(SelectableObject selectableObject)
    {
        if (_listOfSelected.Contains(selectableObject) == false)
        {
            _listOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    void UnSelectAll()
    {
        for (int i = 0; i < _listOfSelected.Count; i++)
        {
            _listOfSelected[i].UnSelect();
        }
        _listOfSelected.Clear();
    }

    void UnhoverCurrent()
    {
        if (_hovered)
        {
            _hovered.UnOnHover();
            _hovered = null;
        }
    }
}
