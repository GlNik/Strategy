using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] private Button _startGameButton, _loadGameButton, _settingButton, _exitButton;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        FadeManager.Instance.LoadGameScene(1);
    }
}
