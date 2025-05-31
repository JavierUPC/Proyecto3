using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource ambient;

    public Slider musicSlider;
    public Slider ambientSlider;

    void Start()
    {
        if (PassSaveFiles.loadedFile != null)
        {
            music.volume = PassSaveFiles.loadedFile.musicVol;
            ambient.volume = PassSaveFiles.loadedFile.ambientVol;
        }
        else
        {
            music.volume = 0.5f;
            ambient.volume = 0.5f;
        }

        musicSlider.value = music.volume;
        ambientSlider.value = ambient.volume;

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        ambientSlider.onValueChanged.AddListener(OnAmbientVolumeChanged);
    }

    void OnMusicVolumeChanged(float value)
    {
        music.volume = value;
        PassSaveFiles.saveFile.musicVol = music.volume;
    }

    void OnAmbientVolumeChanged(float value)
    {
        ambient.volume = value;
        PassSaveFiles.saveFile.ambientVol = ambient.volume;
    }

    void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        ambientSlider.onValueChanged.RemoveListener(OnAmbientVolumeChanged);
    }
}
