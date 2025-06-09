using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public BlackScreen blackScreen;

    public string sceneName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            blackScreen.On();
            StartCoroutine(timeToNextLevel());
        }
    }

    private IEnumerator timeToNextLevel()
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(sceneName);
    }
}
