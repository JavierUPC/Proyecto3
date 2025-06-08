using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamasSecas : MonoBehaviour
{
    private bool activado = false;
    public Animator interactableProp;
    public GameObject fuego;
    private void Start()
    {
        fuego.SetActive(false);    
    }

    public void Activate(TipoMosca type)
    {
        if (type == TipoMosca.Fuego && !activado)
        {
            //Debug.Log("Button Activated");
            activado = true;
            interactableProp.SetTrigger("Activate");
            fuego.SetActive(true);
        }
    }

    public void Eliminate()
    {
        fuego.SetActive(false);
        Destroy(gameObject);
    }
}
