using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    private AudioSource audioSource;
    public float volume = 0f;
    public AudioClip[] audioClip;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Play(int number)
    {
        audioSource.PlayOneShot(audioClip[number]);
    }

    private void Update()
    {
        volume = audioSource.volume;
    }
}
