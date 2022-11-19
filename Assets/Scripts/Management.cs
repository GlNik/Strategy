using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Building,
    EnemyUnitOrStructure,
    Other
}

public class Management : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private SelectableObject _hovered;
    public List<SelectableObject> ListOfSelected;

    [SerializeField] private Image _frameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    private SelectionState _curSelectionState;

    public static Management Instance;

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

    private void Start()
    {
        _curSelectionState = SelectionState.Other;
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.blue);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            SelectableCollider selectableCollider = hit.collider.GetComponent<SelectableCollider>();

            if (selectableCollider)
            {
                SelectableObject hitSelectable = selectableCollider.SelectableObject;
                if (_hovered)
                {
                    if (_hovered != hitSelectable)
                    {
                        _hovered.OnUnhover();

                        _hovered = hitSelectable;
                        _hovered.OnHover();
                    }
                }
                else if (hitSelectable)
                {
                    _hovered = hitSelectable;
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

        if (Input.GetMouseButtonUp(0) && !_frameImage.enabled)
        {
            if (_hovered)
            {
                if (_curSelectionState == SelectionState.Building || !Input.GetKey(KeyCode.LeftControl))
                {
                    if (_hovered.GetComponent<Enemy>() || _hovered.GetComponent<EnemyBuilding>())
                    {
                        if (_curSelectionState != SelectionState.UnitsSelected)
                        {
                            UnselectAll();
                            _curSelectionState = SelectionState.EnemyUnitOrStructure;
                            Select(_hovered);
                        } // иначе ничего не делаем, а передаём управление дальше
                    }
                    else
                    {
                        UnselectAll();
                        _curSelectionState = SelectionState.UnitsSelected;
                        Select(_hovered);
                    }
                }
                else if (Input.GetKey(KeyCode.LeftControl) && !_hovered.GetComponent<Enemy>() && !_hovered.GetComponent<EnemyBuilding>())
                {
                    _curSelectionState = SelectionState.UnitsSelected;
                    Select(_hovered);
                }
            }
        }

        if (_curSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                //if (hit.collider.tag == "Ground")
                //или лучше по слою?
                if (hit.collider.GetComponent<Ground>() != null)
                {
                    int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));
                    Vector3 groupCenter = new Vector3((ListOfSelected.Count - 1) / rowNumber, 0, (ListOfSelected.Count - 1) % rowNumber) / 2f;

                    for (int i = 0; i < ListOfSelected.Count; i++)
                    {
                        int row = i / rowNumber;
                        int column = i % rowNumber;
                        Vector3 point = hit.point + (new Vector3(row, 0f, column) - groupCenter) /** 1f*/;
                        if (ListOfSelected[i] != null)
                        {
                            ListOfSelected[i].WhenClickOnGround(point);
                        }
                    }
                }
                // как избежать GetComponentInParent ?
                if (hit.collider.GetComponentInParent<SelectableObject>())
                {
                    //if (ListOfSelected[0]!=null)
                    if (ListOfSelected.Count > 0 && ListOfSelected[0].GetComponent<Unit>())
                        for (int i = 0; i < ListOfSelected.Count; i++)
                        {
                            if (ListOfSelected[i].GetComponent<Unit>() != null)
                            {
                                // как избежать GetComponentInParent ?
                                ((Unit)ListOfSelected[i]).SetTarget(hit.collider.GetComponentInParent<SelectableObject>());
                            }
                        }
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            UnselectAll();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {

            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            _frameImage.rectTransform.anchoredPosition = min;

            Vector2 size = max - min;

            if (size.magnitude > 10)
            {
                _curSelectionState = SelectionState.Frame;
                _frameImage.enabled = true;

                _frameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                Unit[] allUnits = FindObjectsOfType<Unit>();

                UnselectAll();
                foreach (Unit U in allUnits)
                {
                    Vector2 screenPosition = _camera.WorldToScreenPoint(U.transform.position);
                    if (rect.Contains(screenPosition))
                        Select(U);
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _frameImage.enabled = false;
            if (_curSelectionState != SelectionState.Building && _curSelectionState != SelectionState.EnemyUnitOrStructure)
            {
                if (ListOfSelected.Count > 0)
                    _curSelectionState = SelectionState.UnitsSelected;
                else
                    _curSelectionState = SelectionState.Other;
            }
        }
    }

    private void Select(SelectableObject selectableObject)
    {
        if (selectableObject.GetType().BaseType == typeof(Building))
        {
            UnselectAll();
            _curSelectionState = SelectionState.Building;
        }

        if (!ListOfSelected.Contains(selectableObject))
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject))
            ListOfSelected.Remove(selectableObject);
    }

    public void UnselectAll()
    {
        foreach (SelectableObject S in ListOfSelected)
            S.Unselect();
        ListOfSelected.Clear();
        _curSelectionState = SelectionState.Other;
    }

    private void UnhoverCurrent()
    {
        if (_hovered)
        {
            _hovered.OnUnhover();
            _hovered = null;
        }
    }

    private void SpawnUnit(GameObject UnitPrefab, Transform BuidingPosition)
    {
        Instantiate(UnitPrefab, BuidingPosition);
    }

}