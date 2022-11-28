using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private bool _position;
    [SerializeField] private bool _rotation;

    private void Update()
    {
        if (_position)
        {
            transform.position = _target.position;
        }
        if (_rotation)
        {
            transform.rotation = _target.rotation;
        }
    }
}
