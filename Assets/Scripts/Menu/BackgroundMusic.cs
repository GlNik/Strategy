using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _audio;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.volume = SoundSettingGame.Instance.MusicVolume;

        SoundSettingGame.Instance.CangeSoundMusic.AddListener(UpdateVolumeMusic);
    }

    private void UpdateVolumeMusic(float volume)
    {
        _audio.volume = volume;
    }  
}
