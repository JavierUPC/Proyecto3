using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToBeContinued : MonoBehaviour
{
    public GameObject TBCScreen;
    private Image imageInScreen;
    public float fadeDuration = 1f;

    public string sceneName;

    void Start()
    {
        TBCScreen.SetActive(false);
        imageInScreen = TBCScreen.GetComponent<Image>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PassSaveFiles.Save(SceneManager.GetActiveScene().name);
            StartCoroutine(timeToNextLevel());
            On();
        }
    }

    private IEnumerator timeToNextLevel()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(sceneName);
    }

    public void On()
    {
        TBCScreen.SetActive(true);
        SetAlpha(0f);
        StartCoroutine(FadeAlpha(0f, 1f)); 
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
