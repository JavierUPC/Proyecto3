using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] audioClip;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Play(int number)
    {
        audioSource.PlayOneShot(audioClip[number]);
    }
}
