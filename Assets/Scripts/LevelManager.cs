using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
        {
            Instance = this;            
        }
    }

    public void OpenScene(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }
}
