using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using static Unity.Cinemachine.CinemachineOrbitalFollow;

public class MuerteManager : MonoBehaviour
{
    [Header("Objectes per activar")]
    public GameObject Bra�;
    public Animator animatorMa;

    public CallScreen callScreen;

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
            //POSICIO CAMERA
            freeLookCamera.VerticalAxis.Value = 0.53f;
            freeLookCamera.HorizontalAxis.Value = -114f;

            //OBRIM ANGULACIO
            //freeLookCamera.Orbits.Top.Radius = 39;
            //freeLookCamera.Orbits.Center.Radius = 44;
            //freeLookCamera.Orbits.Bottom.Radius = 39;

            freeLookCamera.Orbits.Top.Radius = 139;
            freeLookCamera.Orbits.Center.Radius = 144;
            freeLookCamera.Orbits.Bottom.Radius = 139;
            //yield return new WaitForSecondsRealtime(10f);
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

        callScreen.Scientist();
    }
}
