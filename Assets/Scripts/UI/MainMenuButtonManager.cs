using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _exitButton;

    [Header("Windows")]
    [SerializeField] private GameObject _startGameWindow;
    [SerializeField] private GameObject _loadGameWindow;
    [SerializeField] private GameObject _settingsWindow;
    [SerializeField] private GameObject _exitGameWindow;
    [SerializeField] private GameObject _buttonsWindowsGroup;


    [Header("Windows Buttons")]
    [SerializeField] private Button _resumeSettingsButton;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _settingButton.onClick.AddListener(() => WindowEnableManager(_settingsWindow));
        _settingButton.onClick.AddListener(() => WindowEnableManager(_buttonsWindowsGroup));

        _resumeSettingsButton.onClick.AddListener(() => WindowEnableManager(_buttonsWindowsGroup));       
        _resumeSettingsButton.onClick.AddListener(() => WindowEnableManager(_settingsWindow));
    }

    private void StartGame()
    {
        FadeManager.Instance.LoadGameScene(1);
    }

    private void WindowEnableManager(GameObject window)
    {
        window.SetActive(!window.activeSelf);
    }
    private void WindowEnableButtonsGroup()
    {

    }
}
