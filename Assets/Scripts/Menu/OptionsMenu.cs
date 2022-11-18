using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;
   // [SerializeField] Slider _volume;

    private float _soundFXFloat;
    private float _soundMusicFloat;

    [SerializeField] AudioSource _music;
    [SerializeField] AudioSource _soundEffects;

   // [SerializeField] private AudioMixer audioMixer;

    void Start()
    {
        _soundFXFloat = SoundSettingGame.Instance.SoundEffectsVolume;
        _sfxSlider.value = _soundFXFloat;
        SetSFXVolume(_soundFXFloat);
        //
        _soundMusicFloat = SoundSettingGame.Instance.MusicVolume;
        _musicSlider.value = _soundMusicFloat;
        SetMusicVolume(_soundMusicFloat);
    }

    public void SetMusicVolume(float volume)
    {
        _music.volume = volume;
        SoundSettingGame.Instance.MusicVolume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        _soundEffects.volume = volume;
        SoundSettingGame.Instance.SoundEffectsVolume = volume;
    }

    private void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("SfxVolume", _sfxSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);

    }

    //public void SetVolume(float volume) {
    //    audioMixer.SetFloat("SoundVolume", VolumeToDb(volume));
    //}



    //public float baseOfLogarithm = 1.15f;
    //public float VolumeToDb(double volume) {
    //    if (volume == 0)
    //        return -80;
    //    else
    //    return (float)Math.Log(volume, baseOfLogarithm);
    //}
    //public float DbToVolume(double db) {
    //    return (float)Math.Pow(baseOfLogarithm, db);
    //}

}
