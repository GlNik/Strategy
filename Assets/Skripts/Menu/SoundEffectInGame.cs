using UnityEngine;

public class SoundEffectInGame : MonoBehaviour
{
    private AudioSource _soundAudio;
    void Start()
    {
        _soundAudio = GetComponent<AudioSource>();
        _soundAudio.volume = SoundSettingGame.Instance.SoundEffectsVolume;

         SoundSettingGame.Instance.CangeSound.AddListener(UpdateVolume);
    }
    private void UpdateVolume(float volume)
    {
        _soundAudio.volume = volume;
    }
    //private void Update()
    //{
    //    _soundAudio.volume = SoundSettingGame.Instance.SoundEffectsVolume;
    //}

    

}
