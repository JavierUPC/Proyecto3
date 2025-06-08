using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MuerteManager : MonoBehaviour
{
    [Header("Objectes per activar")]
    public GameObject Bra�;
    public Animator animatorMa;

    [Header("Camera")]
    public CinemachineOrbitalFollow freeLookCamera;

    private void Start()
    {
        if (Bra� != null)
        {
            Bra�.SetActive(false);
        }
    }

    public void ActivarSecuenciaMuerte()
    {
        StartCoroutine(SecuenciaMuerteCoroutine());
    }

    private IEnumerator SecuenciaMuerteCoroutine()
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

        // 2. Posar la c�mera
        if (freeLookCamera != null)
        {
            freeLookCamera.VerticalAxis.Value = 0.53f;
            freeLookCamera.HorizontalAxis.Value = -114f;
        }

        // 3. Activar animaci�
        if (animatorMa != null)
        {
            animatorMa.SetBool("Agafar", true);
        }
        else
        {
            Debug.LogWarning("No s'ha assignat cap Animator.");
        }

        // 4. Pausar mig segon (0.5s)
        yield return new WaitForSecondsRealtime(1f);

        // 5. Cridar Kill
        Kill.Reload();
    }
}
