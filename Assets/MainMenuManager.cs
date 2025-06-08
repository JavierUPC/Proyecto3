using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject ventanaVolumen;
    public GameObject mainMenu;
    public GameObject creditos;

    private void Start()
    {
        ventanaVolumen.SetActive(false);
        creditos.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void AbrirVolumen()
    {
        ventanaVolumen.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void VolverMenu()
    {
        creditos.SetActive(false);
        ventanaVolumen.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void AbrirCreditos()
    {
        mainMenu.SetActive(false);
        creditos.SetActive(true);
    }
}
