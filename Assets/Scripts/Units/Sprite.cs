using UnityEngine;

public class Sprite : MonoBehaviour
{
    public GameObject Badge;
    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        transform.rotation = _cameraTransform.rotation;
    }

    public void ShowBagde(bool val)
    {
        if (Badge)
            Badge.SetActive(val);
    }
}
