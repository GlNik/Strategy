using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    private bool _pauseGame;
    [SerializeField] private GameObject _pauseGameMenu;
    [SerializeField] private MonoBehaviour[] _componentsToDisable;
    [SerializeField] private GameObject _gameButtons;
    [SerializeField] private GameObject _horizontalPanel;

    private void Start()
    {
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseGame)
            {
                Resume();
                _gameButtons.SetActive(true);
                _horizontalPanel.SetActive(true);
            }
            else
            {
                Pause();
                _gameButtons.SetActive(false);
                _horizontalPanel.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        _pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        _pauseGame = false;
        for (int i = 0; i < _componentsToDisable.Length; i++)
        {
            _componentsToDisable[i].enabled = true;
        }
    }

    public void Pause()
    {
        _pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        _pauseGame = true;
        for (int i = 0; i < _componentsToDisable.Length; i++)
        {
            _componentsToDisable[i].enabled = false;
        }
    }
    public void Lose()
    {
        _gameButtons.SetActive(true);

        Time.timeScale = 0f;
        _pauseGame = true;
        for (int i = 0; i < _componentsToDisable.Length; i++)
        {
            _componentsToDisable[i].enabled = false;
        }
    }
    public void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
