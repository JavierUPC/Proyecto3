using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamasSecas : MonoBehaviour
{
    private bool activado = false;
    public Animator interactableProp;
    public void Activate(TipoMosca type)
    {
        if (type == TipoMosca.Fuego && !activado)
        {
            //Debug.Log("Button Activated");
            activado = true;
            interactableProp.SetTrigger("Activate");
        }
    }
}
