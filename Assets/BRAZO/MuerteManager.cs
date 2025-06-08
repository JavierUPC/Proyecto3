using System.Collections;
using UnityEngine;

public class MuerteManager : MonoBehaviour
{
    [Header("Objectes per activar")]
    public GameObject Braç;
    public Animator animatorMa;

    private void Start()
    {
        if (Braç != null)
        {
            Braç.SetActive(false); //activar i desactivar el braç
        }
    }

    public void ActivarSecuenciaMuerte()
    {
        // 1. Activar el GameObject
        if (Braç != null)
        {
            Braç.SetActive(true);
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
        
        //Kill.Reload(); // Mata al camaleón

    }
}
