using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private bool activado = false;
    public Animator interactableProp;
    public void Activate(TipoMosca type)
    {
        if(type == TipoMosca.Electrico && !activado)
        {
            //Debug.Log("Button Activated");
            activado = true;
            interactableProp.SetTrigger("Activate");
        }
    }
}
