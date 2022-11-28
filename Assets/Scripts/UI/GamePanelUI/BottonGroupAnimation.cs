using UnityEngine;
using DG.Tweening;

public class BottonGroupAnimation : MonoBehaviour
{
    public static BottonGroupAnimation Instance;

    [SerializeField] private Transform _showTransformPosition, _hideTransformPosition;

    [SerializeField] private Transform _buttonPanel;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        AnimationPanel(_showTransformPosition, 1f);
    }

    public void AnimationPanel(Transform transform, float delay)
    {
        _buttonPanel.DOMoveY(transform.position.y, 1).SetDelay(delay);
    }
}
