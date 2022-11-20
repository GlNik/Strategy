using UnityEngine;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    private CanvasGroup _fade;

    private void Awake()
    {
        if (Instance) 
            Destroy(gameObject);
        else
            Instance = this;

        _fade = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _fade.alpha = 1;
        HideFade();
    }

    public void ShowFade()
    {
        _fade.DOFade(1, 1);
    }

    public void HideFade()
    {
        _fade.DOFade(0, 1).SetDelay(1);
    }

    public void LoadGameScene(int index)
    {
        Tween tween = _fade.DOFade(1, 1);
        tween.onComplete += () =>
        {
            LevelManager.Instance.OpenScene(index);
        };
    }
}
