using UnityEngine;
using UnityEngine.Events;

public class SoundSettingGame : MonoBehaviour
{
    public static SoundSettingGame Instance;
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _soundEffectsVolume;

    public UnityEvent<float> CangeSound;
    public UnityEvent<float> CangeSoundMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Load()
    {
        //_musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        //_soundEffectsVolume = PlayerPrefs.GetFloat("SfxVolume", 0.3f);

        SoundEffectsVolume = PlayerPrefs.GetFloat("SfxVolume", 0.7f); 
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f); 
    }
    public float MusicVolume
    {
        get { return _musicVolume; }
        set
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            _musicVolume = value;
            CangeSoundMusic.Invoke(_musicVolume);
        }
    }

    public float SoundEffectsVolume
    {
        get { return _soundEffectsVolume; }
        set
        {
            PlayerPrefs.SetFloat("SfxVolume", value);
            _soundEffectsVolume = value;
            CangeSound.Invoke(_soundEffectsVolume);
        }
    }

}
