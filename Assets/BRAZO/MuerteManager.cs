using System.Collections;
using UnityEngine;

public class MuerteManager : MonoBehaviour
{
    [Header("Objectes per activar")]
    public GameObject Bra�;
    public Animator animatorMa;

    private void Start()
    {
        if (Bra� != null)
        {
            Bra�.SetActive(false); //activar i desactivar el bra�
        }
    }

    public void ActivarSecuenciaMuerte()
    {
        // 1. Activar el GameObject
        if (Bra� != null)
        {
            Bra�.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No s'ha assignat cap objecte a activar.");
        }

        // 2. Activar el bool 'agafar' de l'Animator
        if (animatorMa != null)
        {
            animatorMa.SetBool("Agafar", true);
        }
        else
        {
            Debug.LogWarning("No s'ha assignat cap Animator.");
        }
        
        //Kill.Reload(); // Mata al camale�n

    }
}
