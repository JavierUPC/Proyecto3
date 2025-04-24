using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ScientistController : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject scientist; // Cient�fico
    public GameObject chameleon; // Camale�n

    [Header("Par�metros")]
    public Vector3 spawnPosition;
    public Vector3 exitPosition;
    public float timeBetweenCycles = 10f;
    public float timeBeforeExit = 5f;
    public float detectionDistance = 3f;

    [Header("Colores")]
    public Color defaultColor = Color.white;
    public Color alertColor = Color.red;

    private NavMeshAgent agent;
    private Renderer scientistRenderer; // Cambiar color del cient�fico

    private void Start()
    {
        if (scientist != null)
        {
            agent = scientist.GetComponent<NavMeshAgent>();
            scientistRenderer = scientist.GetComponent<Renderer>(); // Acceder al MeshRenderer del cient�fico
            scientistRenderer.material.color = defaultColor;
            StartCoroutine(ScientistCycle());
        }
    }

    private IEnumerator ScientistCycle()
    {
        while (true)
        {
            // Spawn del cient�fico
            scientist.transform.position = spawnPosition;
            scientist.SetActive(true);
            scientistRenderer.material.color = defaultColor;

            // **Actualizar constantemente el destino del cient�fico hacia el camale�n**
            bool alertTriggered = false;
            float elapsed = 0f;

            while (elapsed < timeBeforeExit)
            {
                // Establecer el destino constantemente
                agent.SetDestination(chameleon.transform.position);

                // Comprobar la distancia para cambiar el color
                float distance = Vector3.Distance(scientist.transform.position, chameleon.transform.position);
                if (distance <= detectionDistance && !alertTriggered)
                {
                    scientistRenderer.material.color = alertColor; // Cambiar color cuando se acerca
                    alertTriggered = true;
                }
                else if (distance > detectionDistance && alertTriggered)
                {
                    scientistRenderer.material.color = defaultColor; // Restaurar el color cuando se aleja
                    alertTriggered = false;
                }

                elapsed += Time.deltaTime;
                yield return null; // Esperar hasta el siguiente frame
            }

            // Restaurar color antes de ir al exit
            scientistRenderer.material.color = defaultColor;

            // Ir a la posici�n de salida
            agent.SetDestination(exitPosition);

            // Esperar hasta que llegue (aproximadamente)
            while (Vector3.Distance(scientist.transform.position, exitPosition) > 0.5f)
            {
                yield return null;
            }

            // Desaparecer
            scientist.SetActive(false);

            // Esperar hasta el pr�ximo ciclo
            yield return new WaitForSeconds(timeBetweenCycles);
        }
    }
}
