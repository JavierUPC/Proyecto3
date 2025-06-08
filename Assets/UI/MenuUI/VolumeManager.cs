using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource[] ambient;

    public Slider musicSlider;
    public Slider ambientSlider;

    void Start()
    {
        float musicVol = 0.5f;
        float ambientVol = 0.5f;

        if (PassSaveFiles.loadedFile != null)
        {
            musicVol = PassSaveFiles.loadedFile.musicVol;
            ambientVol = PassSaveFiles.loadedFile.ambientVol;
        }

        music.volume = musicVol;
        foreach (AudioSource amb in ambient)
            amb.volume = ambientVol;

        musicSlider.value = musicVol;
        ambientSlider.value = ambientVol;

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        ambientSlider.onValueChanged.AddListener(OnAmbientVolumeChanged);
    }

    void OnMusicVolumeChanged(float value)
    {
        music.volume = value;
        PassSaveFiles.saveFile.musicVol = value;
    }

    void OnAmbientVolumeChanged(float value)
    {
        foreach (AudioSource amb in ambient)
            amb.volume = value;

        PassSaveFiles.saveFile.ambientVol = value;
    }

    void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        ambientSlider.onValueChanged.RemoveListener(OnAmbientVolumeChanged);
    }
}
