using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuerteManager : MonoBehaviour
{
    public Camera camaraPrincipal;
    public Transform posicionCamaraMuerte;
    public Animator manoAnimacion; // Animació de la mà que agafa el camaleó
    public float tiempoAntesDeKill = 1f;

    private bool muerteEnProgreso = false;

    public void ActivarSecuenciaMuerte()
    {
        if (muerteEnProgreso) return;
        muerteEnProgreso = true;
        StartCoroutine(SecuenciaMuerte());
    }

    private IEnumerator SecuenciaMuerte()
    {
        // 1. Col·locar càmera en la posició concreta
        if (camaraPrincipal != null && posicionCamaraMuerte != null)
        {
            camaraPrincipal.transform.position = posicionCamaraMuerte.position;
            camaraPrincipal.transform.rotation = posicionCamaraMuerte.rotation;
        }

        // 2. Llençar animació de la mà si n'hi ha
        if (manoAnimacion != null)
        {
            manoAnimacion.SetTrigger("Agafar"); // Assegura’t que el trigger es diu així
        }

        // 3.Parar el camaleon. Que no es pugui moure. Desacrivar els scripts de moviment
        Time.timeScale = 0f;

        // 4. Esperar 1 segon real (no afectat pel timeScale)
        yield return new WaitForSecondsRealtime(tiempoAntesDeKill);

        // 5. Tornar al timeScale normal
        Time.timeScale = 1f;

        // 6. Cridar Kill
        Kill.Reload();
    }
}
