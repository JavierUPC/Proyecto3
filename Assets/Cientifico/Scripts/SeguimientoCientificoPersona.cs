using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeguimientoCientificoPersona : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject camaleon;

    [Header("Posiciones")]
    public Vector3 spawnPosition;
    public Vector3 exitPosition;

    [Header("Tiempos")]
    public float tiempoEsperaEntreCiclos = 2f;
    public float tiempoDeteccion = 2f;
    public float tiempoNecesarioParaMatar = 3f;

    [Header("Detección")]
    public float distanciaDeteccion = 5f;

    [Header("Colores")]
    public Color colorNormal = Color.white;
    public Color colorDeteccion = Color.red;

    private NavMeshAgent agent;
    private Renderer rend;
    private Camuflaje camuflajeScript; // Referencia al script Camuflaje para la variable bool isCamo

    private float tiempoDeteccionActual;
    private float tiempoMatarActual;
    private bool buscando = false;
    private bool enFaseDeteccion = false;
    private bool yendoASalida = false;
    private bool yendoASpawn = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();
        camuflajeScript = camaleon.GetComponent<Camuflaje>();
       
        StartCoroutine(Ciclo());
    }

    private System.Collections.IEnumerator Ciclo()
    {
        while (true)
        {

            yield return new WaitForSeconds(tiempoEsperaEntreCiclos);

            //Buscar camaleón
            buscando = true;
            enFaseDeteccion = false;
            yendoASalida = false;
            yendoASpawn = false;
            tiempoDeteccionActual = tiempoDeteccion;
            tiempoMatarActual = tiempoNecesarioParaMatar;

            while (buscando)
            {
                agent.SetDestination(new Vector3(camaleon.transform.position.x, transform.position.y, camaleon.transform.position.z));

                float distancia = DistanciaXZ(transform.position, camaleon.transform.position);

                if (distancia <= distanciaDeteccion)
                {
                    //Iniciar tiempo de detección
                    buscando = false;
                    enFaseDeteccion = true;
                }

                yield return null;
            }

            while (enFaseDeteccion)
            {
                agent.SetDestination(new Vector3(camaleon.transform.position.x, transform.position.y, camaleon.transform.position.z));

                if (tiempoDeteccionActual > 0)
                {
                    tiempoDeteccionActual -= Time.deltaTime;

                    if (!camuflajeScript.isCamo)
                    {
                        // No camuflado -> detección
                        rend.material.color = colorDeteccion;
                        tiempoMatarActual -= Time.deltaTime;

                        if (tiempoMatarActual <= 0)
                        {
                            Kill.Reload();
                            //Debug.Log("MUERTE CAMALEON");
                            rend.material.color = colorNormal;
                            enFaseDeteccion = false;
                        }
                    }
                    else
                    {
                        // Camuflado -> reset matar
                        rend.material.color = colorNormal;
                        tiempoMatarActual = tiempoNecesarioParaMatar;
                    }
                }
                else
                {
                    // Fin tiempo de detección
                    enFaseDeteccion = false;
                }

                yield return null;
            }

            //Ir a salida
            yendoASalida = true;
            agent.SetDestination(new Vector3(exitPosition.x, transform.position.y, exitPosition.z));

            while (yendoASalida)
            {
                if (DistanciaXZ(transform.position, exitPosition) <= 1f)
                {
                    yendoASalida = false;
                }
                yield return null;
            }

            //Ir a spawn
            yendoASpawn = true;
            agent.SetDestination(new Vector3(spawnPosition.x, transform.position.y, spawnPosition.z));

            while (yendoASpawn)
            {
                if (DistanciaXZ(transform.position, spawnPosition) <= 1f)
                {
                    yendoASpawn = false;
                }
                yield return null;
            }
        }
    }

    private float DistanciaXZ(Vector3 a, Vector3 b)
    {
        Vector2 a2D = new Vector2(a.x, a.z);
        Vector2 b2D = new Vector2(b.x, b.z);
        return Vector2.Distance(a2D, b2D);
    }
}
