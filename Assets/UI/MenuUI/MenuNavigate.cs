using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigate : MonoBehaviour
{
    public GameObject menuWindow, volumeWindow, controlsWindow;

    private void Start()
    {
    }

    public void OpenVolume()
    {
        volumeWindow.SetActive(true);
        menuWindow.SetActive(false);
        controlsWindow.SetActive(false);
    }

    public void OpenControls()
    {
        controlsWindow.SetActive(true);
        menuWindow.SetActive(false);
        volumeWindow.SetActive(false);
        Debug.Log("OpenControls");
    }

    public void BackToMenu()
    {
        menuWindow.SetActive(true);
        volumeWindow.SetActive(false);
        controlsWindow.SetActive(false);
    }
}
