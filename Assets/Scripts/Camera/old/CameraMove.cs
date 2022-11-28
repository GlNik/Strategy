using UnityEngine;
using UnityEngine.EventSystems;


public class CameraMove : MonoBehaviour
{
    private Camera _raycastCamera;
    
    private float _startMouseX;
    private float _startMouseY;
  
    private Quaternion _cameraStartRotation;
    private Plane _plane;
   //
    [SerializeField] private float _screenEdgeSpeed = 30f;
    [SerializeField] private float _screenEdgeBorderSize = 30f;
    [SerializeField] private float _keyBoardSpeed = 30f;
    // click drag
    //[SerializeField] float _dragSpeed = 500f;
    //KeyCode _dragKey = KeyCode.Mouse1;
    private Transform _mainTransform;
    [SerializeField] private float _minY = 3.5f;
    [SerializeField] private float _maxY = 50f;
    [SerializeField] private Vector2 _limit;
    [SerializeField] private float _scrollSpeed = 1200f;

    [SerializeField] private float _zoomSensitivity;
    [SerializeField] private float _zoomSmoothig;
    LayerMask _groundMask;
    private float _zoomAmount;
    //

    private void Awake()
    {
        _raycastCamera = Camera.main;
    }

    private void Start()
    {
        _mainTransform = transform;
        _plane = new Plane(Vector3.up, Vector3.zero);
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        Vector3 position = _mainTransform.position;
        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        if (Input.GetMouseButtonDown(2))
        {
            //_startPoint = point;
            _startMouseX = Input.mousePosition.x;
            _startMouseY = Input.mousePosition.y;
            _cameraStartRotation = transform.rotation;
        }

        if (Input.GetMouseButton(2))
        {
            {
                float _cameraStartRotationX =
                    _cameraStartRotation.eulerAngles.x > 180 ? _cameraStartRotation.eulerAngles.x - 360 : _cameraStartRotation.eulerAngles.x;


                transform.rotation = Quaternion.Euler(
               Mathf.Clamp(_cameraStartRotationX - (Input.mousePosition.y - _startMouseY) / Screen.height * 160f, -10f, 90f),
               _cameraStartRotation.eulerAngles.y + (Input.mousePosition.x - _startMouseX) / Screen.width * 360f,
               _cameraStartRotation.eulerAngles.z);

            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        position.y -= scroll * _scrollSpeed * 100f * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -_limit.x, _limit.x);
        position.y = Mathf.Clamp(position.y, _minY, _maxY);
        position.z = Mathf.Clamp(position.z, -_limit.y, _limit.y);
        transform.position = position;
        Move();
        HeightCalculation();
    }

    private void Move()
    {
        //опционально пока думаю добавлять ли такой способ)
        //if (Input.GetKey(_dragKey))
        //{
        //    // click drag
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

    private void HeightCalculation()
    {
        _zoomAmount += -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * _zoomSensitivity;
        _zoomAmount = Mathf.Clamp01(_zoomAmount);

        float distanceToGround = DistanceToGround();
        float targetHeight = Mathf.Lerp(_minY, _maxY, _zoomAmount);

        _mainTransform.position = Vector3.Lerp(_mainTransform.position,
            new Vector3(_mainTransform.position.x, targetHeight + distanceToGround, _mainTransform.position.z),
            Time.deltaTime * _zoomSmoothig);
    }

    private float DistanceToGround()
    {
        Ray ray = new Ray(_mainTransform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundMask))
        {
            return hit.point.y;
        }
        return 0;
    }
}
