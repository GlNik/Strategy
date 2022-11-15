using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    private bool _pauseGame;
    [SerializeField] private GameObject _pauseGameMenu;
    [SerializeField] MonoBehaviour[] _componentsToDisable;
    [SerializeField] private GameObject _endGame;
   // [SerializeField] RandomSound _randomSound;
    private void Start()
    {
        Time.timeScale = 1f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        //if (ScoreManager.Instance._score <= 0)
        //{
        //    Lose();
        //}
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
        _endGame.SetActive(true);
        //_randomSound.EndGameMusic = true;
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
