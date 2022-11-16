using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] float _keyBoardSpeed;
    [SerializeField] float _dragSpeed;
    [SerializeField] float _screenEdgeSpeed;
    [SerializeField] float _screenEdgeBorderSize;
    [SerializeField] float _mouseRotationSpeed;
    [SerializeField] float _followMoveSpeed;
    [SerializeField] float _followRotationSpeed;
    [SerializeField] float _minHeight = 8f;
    [SerializeField] float _maxHeight = 60f;
    [SerializeField] float _zoomSensitivity;
    [SerializeField] float _zoomSmoothing;
    [SerializeField] float _mapLimitSmoothing;

    [SerializeField] Vector2 _mapLimits;
    [SerializeField] Vector2 _rotationLimits;

    [SerializeField] Vector3 _followOffset;

    Transform _targetToFollow;

    float _zoomAmout = 1f;
    float _yaw;
    float _pitch;

    KeyCode _dragKey = KeyCode.Mouse1;
    KeyCode _rotationKey = KeyCode.Mouse2;

    Transform _mainTransform;
    LayerMask _groundMask;

    void Start()
    {
        _mainTransform = transform;
        _groundMask = LayerMask.GetMask("Ground");
        _pitch = _mainTransform.eulerAngles.x;
    }

    void Update()
    {
        if (!_targetToFollow)
        {
            Move();
        }
        else
        {
            FollowTarget();
        }

        Rotation();
        HeightCalculation();
        LimitPosition();

        if(Input.GetKey(KeyCode.Escape))
        {
            ResetTarget();
        }
    }

    void Move()
    {
        if (Input.GetKey(_dragKey))
        {
            //click drag
            Vector3 desiredDragMove = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y")) * _dragSpeed;
            desiredDragMove = Quaternion.Euler(new Vector3(0, _mainTransform.eulerAngles.y, 0)) * desiredDragMove * Time.deltaTime;
            desiredDragMove = _mainTransform.InverseTransformDirection(desiredDragMove);

            _mainTransform.Translate(desiredDragMove, Space.Self);
        }
        else
        {
            //keyboard move
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

    void Rotation()
    {
        if (Input.GetKey(_rotationKey))
        {
            _yaw += _mouseRotationSpeed * Input.GetAxis("Mouse X");
            _pitch -= _mouseRotationSpeed * Input.GetAxis("Mouse Y");

            _pitch = Mathf.Clamp(_pitch, _rotationLimits.x, _rotationLimits.y);

            _mainTransform.eulerAngles = new Vector3(_pitch, _yaw, 0);
        }
    }

    void LimitPosition()
    {
        _mainTransform.position = Vector3.Lerp(_mainTransform.position, new Vector3(
            Mathf.Clamp(_mainTransform.position.x, -_mapLimits.x, _mapLimits.x), _mainTransform.position.y,
            Mathf.Clamp(_mainTransform.position.z, -_mapLimits.y, _mapLimits.y)), Time.deltaTime * _mapLimitSmoothing);
    }

    void HeightCalculation()
    {
        _zoomAmout += -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * _zoomSensitivity;
        _zoomAmout = Mathf.Clamp01(_zoomAmout);

        float distanceToGround = DistanceToGround();
        float targetHeight = Mathf.Lerp(_minHeight, _maxHeight, _zoomAmout);

        _mainTransform.position = Vector3.Lerp(_mainTransform.position,
            new Vector3(_mainTransform.position.x, targetHeight + distanceToGround, _mainTransform.position.z),
            Time.deltaTime * _zoomSmoothing);
    }

    float DistanceToGround()
    {
        Ray ray = new Ray(_mainTransform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundMask))
        {
            return hit.point.y;
        }
        return 0;
    }

    void FollowTarget()
    {
        Vector3 targetPos = new Vector3(_targetToFollow.position.x, _mainTransform.position.y, _targetToFollow.position.z) + _followOffset;
        _mainTransform.position = Vector3.MoveTowards(_mainTransform.position, targetPos, Time.deltaTime * _followMoveSpeed);

        if(_followRotationSpeed>0 && !Input.GetKey(_rotationKey))
        {
            Vector3 targetDirection = (_targetToFollow.position - _mainTransform.position).normalized;
            Quaternion targetRotation = Quaternion.Lerp(_mainTransform.rotation, Quaternion.LookRotation(targetDirection),
                _followRotationSpeed* Time.deltaTime);

            _mainTransform.rotation = targetRotation;

            _pitch = _mainTransform.eulerAngles.x;
            _yaw = _mainTransform.eulerAngles.y;
        }
    }

    public void SetTarget(Transform target)
    {
        _targetToFollow = target;
    }

    public void ResetTarget()
    {
        _targetToFollow = null;
    }
}
