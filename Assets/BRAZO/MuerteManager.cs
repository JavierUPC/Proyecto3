using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MuerteManager : MonoBehaviour
{
    [Header("Objectes per activar")]
    public GameObject Braç;
    public Animator animatorMa;

    [Header("Camera")]
    public CinemachineOrbitalFollow freeLookCamera;

    private void Start()
    {
        if (Braç != null)
        {
            Braç.SetActive(false);
        }
    }

    public void ActivarSecuenciaMuerte()
    {
        StartCoroutine(SecuenciaMuerteCoroutine());
    }

    private IEnumerator SecuenciaMuerteCoroutine()
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

        // 2. Posar la càmera
        if (freeLookCamera != null)
        {
            freeLookCamera.VerticalAxis.Value = 0.53f;
            freeLookCamera.HorizontalAxis.Value = -114f;
        }

        // 3. Activar animació
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
