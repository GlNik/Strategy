using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Camera _raycastCamera;
    [SerializeField] GameObject _cameraCenter;
    [SerializeField] GameObject _raycastCameraCenter;

    private Vector3 _startPoint;
    private float _startMouseX;
    private float _startMouseY;
    private Vector3 _cameraStartPosition;
    private Quaternion _cameraStartRotation;
    private Plane _plane;

    // доп настройки
    [SerializeField] float _screenEdgeSpeed=30f;
    [SerializeField] float _screenEdgeBorderSize=30f;
    [SerializeField] float _keyBoardSpeed = 30f;

    [SerializeField] float _dragSpeed=500f;
    KeyCode _dragKey = KeyCode.Mouse1;
    Transform _mainTransform;
    [SerializeField] float _minY = 3.5f;
    [SerializeField] float _maxY = 50f;
    [SerializeField] Vector2 _limit;
    [SerializeField] float _scrollSpeed = 1200f;
    //

    void Start()
    {
        _mainTransform = transform;
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        Vector3 position = _mainTransform.position;
        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        if (Input.GetMouseButtonDown(2))
        {
            _startPoint = point;
            _startMouseX = Input.mousePosition.x;
            _startMouseY = Input.mousePosition.y;
            _cameraStartPosition = _cameraCenter.transform.position;
            _cameraStartRotation = _cameraCenter.transform.rotation;
        }

        if (Input.GetMouseButton(2))
        {            
            {
                float _cameraStartRotationX =
                    _cameraStartRotation.eulerAngles.x > 180 ? _cameraStartRotation.eulerAngles.x - 360 : _cameraStartRotation.eulerAngles.x;

                _cameraCenter.transform.rotation = Quaternion.Euler(
                    Mathf.Clamp(_cameraStartRotationX - (Input.mousePosition.y - _startMouseY) / Screen.height * 160f, -10f, 90f),
                    _cameraStartRotation.eulerAngles.y + (Input.mousePosition.x - _startMouseX) / Screen.width * 360f,
                    _cameraStartRotation.eulerAngles.z);

                _raycastCameraCenter.transform.rotation = _cameraCenter.transform.rotation;
            }            
        }


        //if (Input.mouseScrollDelta.y != 0)
        //{
        //    transform.Translate(0f, 0f, Input.mouseScrollDelta.y);

        //    if (Input.mouseScrollDelta.y > 0 && transform.position.y < _minY) // ограничение приближения
        //        transform.Translate(0f, 0f, -Input.mouseScrollDelta.y);
        //    else if(Input.mouseScrollDelta.y < 0 && transform.position.y > _maxY) // ограничение удаления
        //        transform.Translate(0f, 0f, -Input.mouseScrollDelta.y);
        //    else
        //        _raycastCamera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        //}

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        position.y -= scroll * _scrollSpeed * 100f * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -_limit.x, _limit.x);
        position.y = Mathf.Clamp(position.y, _minY, _maxY);
        position.z = Mathf.Clamp(position.z, -_limit.y, _limit.y);
        transform.position = position;
        Move();

    }

    private void Move()
    {
        //опционально пока думаю добавлять ли такой способ)
        //if (Input.GetKey(_dragKey))
        //{
        //    //click drag
        //    Vector3 desiredDragMove = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y")) * _dragSpeed;
        //    desiredDragMove = Quaternion.Euler(new Vector3(0, _mainTransform.eulerAngles.y, 0)) * desiredDragMove * Time.deltaTime;
        //    desiredDragMove = _mainTransform.InverseTransformDirection(desiredDragMove);

        //    _mainTransform.Translate(desiredDragMove, Space.Self);
        //}
        
        
            // WSAD
            Vector3 desiredMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            desiredMove *= _keyBoardSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0, _mainTransform.eulerAngles.y, 0)) * desiredMove;
            desiredMove = _mainTransform.InverseTransformDirection(desiredMove);

            _mainTransform.Translate(desiredMove, Space.Self);


            // screen edge move
            Vector3 desiredEdgeMove = new Vector3();
            Vector3 mousePos = Input.mousePosition;

            Rect leftRect = new Rect(0, 0, _screenEdgeBorderSize, Screen.height);
            Rect rightRect = new Rect(Screen.width - _screenEdgeBorderSize, 0, _screenEdgeBorderSize, Screen.height);
            Rect upRect = new Rect(0, Screen.height - _screenEdgeBorderSize, Screen.width, _screenEdgeBorderSize);
            Rect downRect = new Rect(0, 0, Screen.width, _screenEdgeBorderSize);

            desiredEdgeMove.x = leftRect.Contains(mousePos) ? -1 : rightRect.Contains(mousePos) ? 1 : 0;
            desiredEdgeMove.z = upRect.Contains(mousePos) ? 1 : downRect.Contains(mousePos) ? -1 : 0;

            desiredEdgeMove *= _screenEdgeSpeed;
            desiredEdgeMove *= Time.deltaTime;
            desiredEdgeMove = Quaternion.Euler(new Vector3(0, _mainTransform.eulerAngles.y, 0)) * desiredEdgeMove;
            desiredEdgeMove = _mainTransform.InverseTransformDirection(desiredEdgeMove);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _mainTransform.Translate(desiredEdgeMove, Space.Self);
            }
        
    }     
}
