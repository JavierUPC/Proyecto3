using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Habilidad : MonoBehaviour
{
    private TipoMosca tipo = TipoMosca.None;
    public InputActionReference fire;
    private bool habilidad;


    public void AssignType(TipoMosca type)
    {
        tipo = type;
        habilidad = true;
    }

    public void Stop()
    {
        habilidad = false;
        tipo = TipoMosca.None;
    }
}
