using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public GameObject blackScreen;
    private Image imageInScreen;
    public float fadeDuration = 1f;

    void Start()
    {
        blackScreen.SetActive(true); // Make sure it's on
        imageInScreen = blackScreen.GetComponent<Image>();

        SetAlpha(1f); // Start fully opaque (black)
        StartCoroutine(FadeOutAndDeactivate()); // Then fade to transparent
    }

    public void On()
    {
        blackScreen.SetActive(true);
        StopAllCoroutines();
        SetAlpha(0f); // Make sure it starts transparent
        StartCoroutine(FadeAlpha(0f, 1f)); // Fade in
    }

    public void Off()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndDeactivate()); // Fade out and disable
    }

    private IEnumerator FadeOutAndDeactivate()
    {
        yield return FadeAlpha(1f, 0f); // Fade out
        blackScreen.SetActive(false); // Disable after fade
    }

    private IEnumerator FadeAlpha(float from, float to)
    {
        float elapsed = 0f;
        Color color = imageInScreen.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
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
}
