using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Habilidad : MonoBehaviour
{
    private TipoMosca tipo = TipoMosca.None;
    public InputActionReference fire;
    private bool habilidad;

    private void OnEnable()
    {
        fire.action.Enable();

        fire.action.performed += Fire;
    }

    private void OnDisable()
    {
        fire.action.Disable();

        fire.action.performed -= Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (habilidad)
        {
            //meter los distintos tipos de habilidades aquí
            Debug.Log("Se usó la soguiente habilidad: " + tipo);
        }
    }

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
