using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CallScreen : MonoBehaviour
{
    public GameObject scientist, electricity, cactus;
    private Image imageInScreen;
    public float fadeDuration = 1f;
    public Aim aim;
    public PlayerInput playerInput;
    public float killWaitTime;
    public AudioSource musica;
    public AudioClip audioClip;

    private void Start()
    {
        scientist.SetActive(false);
        electricity.SetActive(false);
        cactus.SetActive(false);
    }

    public void Scientist()
    {
        imageInScreen = scientist.GetComponent<Image>();
        On(scientist);
    }

    public void Electricity()
    {
        imageInScreen = electricity.GetComponent<Image>();
        On(electricity);
    }

    public void Cactus()
    {
        imageInScreen = cactus.GetComponent<Image>();
        On(cactus);
    }

    public void On(GameObject screen)
    {
        musica.Stop();
        musica.PlayOneShot(audioClip);
        StartCoroutine(KillWait());
        aim.enabled = false;
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Disable();
        Time.timeScale = 0f;

        screen.SetActive(true);
        SetAlpha(0f); 
        StartCoroutine(FadeAlpha(0f, 1f)); 
    }

    private IEnumerator FadeAlpha(float from, float to)
    {
        float elapsed = 0f;
        Color color = imageInScreen.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            color.a = alpha;
            imageInScreen.color = color;
            yield return null;
        }

        color.a = to;
        imageInScreen.color = color;
    }

    private void SetAlpha(float alpha)
    {
        Color color = imageInScreen.color;
        color.a = alpha;
        imageInScreen.color = color;
    }

    private IEnumerator KillWait()
    {
        yield return new WaitForSecondsRealtime(killWaitTime);
        Kill.Reload();
    }
}
