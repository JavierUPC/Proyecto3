using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject ventanaVolumen;
    public GameObject mainMenu;

    private void Start()
    {
        ventanaVolumen.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void AbrirVolumen()
    {
        ventanaVolumen.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void VolverMenu()
    {
        ventanaVolumen.SetActive(false);
        mainMenu.SetActive(true);
    }
}
