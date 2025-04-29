using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string sceneName;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Rejilla"));
            SceneManager.LoadScene(sceneName);
    }
}
