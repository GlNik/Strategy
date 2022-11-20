using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _centrObject;
    [SerializeField] private float _X, _Y, _Z;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _curve;

    private int direction = 1;
    Transform _cameraTransform;
    private Vector3 _startPosition;

    private void Awake()
    {
        _cameraTransform = transform;
        _startPosition = _cameraTransform.position;
    }

    private void Start()
    {
        CameraMove();
    }

    private void Update()
    {
        transform.LookAt(_centrObject);
    }

    private void CameraMove()
    {
        direction *= -1;
        int directionY = Random.Range(-1f, 1f) > 0 ? 1 : -1;
        // Vector3 randomDirection = new Vector3(Random.Range(_X, _X * 3) * direction, Random.Range(_Y, _Y * 3) * direction, Random.Range(_Z, _Z * 3) * direction);
        Vector3 randomDirection = new Vector3(Random.Range(_X, _X * 3) * direction, Random.Range(_Y, _Y * 3) * directionY, Random.Range(_Z, _Z * 3) * direction);
        Vector3 newPosition = _startPosition + randomDirection;

        float distance = Vector3.Distance(_cameraTransform.position, newPosition);

        Tween tween = _cameraTransform.DOMove(newPosition, distance / _speed).SetEase(_curve);
        tween.onComplete += () =>
        {
            CameraMove();
        };
    }
}
