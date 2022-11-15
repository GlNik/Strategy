using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other
}

public class Managment : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] SelectableObject _hovered;
    [SerializeField] List<SelectableObject> _listOfSelected = new List<SelectableObject>();

    [SerializeField] Image _frameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    [SerializeField] SelectionState _currentSelectionState;

    private void Start()
    {
        _frameImage.enabled = false;
    }
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
                _currentSelectionState = SelectionState.UnitsSelected;
                Select(_hovered);
            }
        }

        if (_currentSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.tag == "Ground")
                {
                    for (int i = 0; i < _listOfSelected.Count; i++)
                    {
                        _listOfSelected[i].WhenClickOnGround(hit.point);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            UnSelectAll();
        }

        //выделение рамкой
        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {

            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 size = max - min;

            if (size.magnitude > 10)
            {
                _frameImage.enabled = true;

                _frameImage.rectTransform.anchoredPosition = min;

                _frameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                UnSelectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++)
                {
                    Vector2 screenPosition = _camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPosition))
                    {
                        Select(allUnits[i]);
                    }
                }
                _currentSelectionState = SelectionState.Frame;
            }


        }
        if (Input.GetMouseButtonUp(0))
        {
            _frameImage.enabled = false;
            if (_listOfSelected.Count > 0)
            {
                _currentSelectionState = SelectionState.UnitsSelected;
            }
            else
            {
                _currentSelectionState = SelectionState.Other;
            }
        }
        //

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
        _currentSelectionState = SelectionState.Other;
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
