using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Kill
{
    public static void HandKill()
    {
    }

    public static void Reload()
    {
        Debug.Log("Entra en Kill");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
