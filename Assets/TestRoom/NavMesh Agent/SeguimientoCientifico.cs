using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CientificoPersecucion : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject camaleon; // el camaleón real (el que se mata)
    public GameObject objetivoPersecucion; // el empty fuera del terrario

    [Header("Posiciones")]
    public Vector3 spawnPosition;
    public Vector3 exitPosition;

    [Header("Tiempos")]
    public float tiempoEsperaEntreCiclos = 2f;
    public float tiempoDeteccion = 2f;
    public float tiempoNecesarioParaMatar = 3f;

    [Header("Detección")]
    public float distanciaDeteccion = 5f;

    [Header("Animación")]
    public float umbralVelocidad = 0.2f;
    public float velocidadMinima = 20f;
    public float velocidadMaxima = 60f;

    [Header("Materiales")]
    public Material materialNormal;
    public Material materialDeteccion;

    private NavMeshAgent agent;
    public Renderer rend;
    private Camuflaje camuflajeScript;
    private Animator animator;

    private float tiempoDeteccionActual;
    private float tiempoMatarActual;
    private bool buscando = false;
    private bool enFaseDeteccion = false;
    private bool yendoASalida = false;
    private bool yendoASpawn = false;

    //public SFX_Manager sfx;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        camuflajeScript = camaleon.GetComponent<Camuflaje>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(Ciclo());
    }

    private void Update()
    {
        float velocidadActual = agent.velocity.magnitude;
        float velocidadNormalizada = Mathf.InverseLerp(velocidadMinima, velocidadMaxima, velocidadActual);
        animator.SetFloat("Speed", velocidadNormalizada);

        bool estaCaminando = velocidadActual > umbralVelocidad;
        animator.SetBool("isWalking", estaCaminando);
    }

    private IEnumerator Ciclo()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEsperaEntreCiclos);

            buscando = true;
            enFaseDeteccion = false;
            yendoASalida = false;
            yendoASpawn = false;
            tiempoDeteccionActual = tiempoDeteccion;
            tiempoMatarActual = tiempoNecesarioParaMatar;

            // BÚSQUEDA
            while (buscando)
            {
                Vector3 destino = new Vector3(objetivoPersecucion.transform.position.x, transform.position.y, objetivoPersecucion.transform.position.z);
                agent.SetDestination(destino);

                float distancia = DistanciaXZ(transform.position, objetivoPersecucion.transform.position);

                if (distancia <= distanciaDeteccion)
                {
                    buscando = false;
                    enFaseDeteccion = true;
                }

                yield return null;
            }

            // DETECCIÓN
            while (enFaseDeteccion)
            {
                Vector3 destino = new Vector3(objetivoPersecucion.transform.position.x, transform.position.y, objetivoPersecucion.transform.position.z);
                agent.SetDestination(destino);

                if (tiempoDeteccionActual > 0)
                {
                    tiempoDeteccionActual -= Time.deltaTime;

                    if (!camuflajeScript.isCamo)
                    {
                        rend.material = materialDeteccion;
                        tiempoMatarActual -= Time.deltaTime;

                        if (tiempoMatarActual <= 0)
                        {
                            Kill.Reload(); // Mata al camaleón
                            rend.material = materialNormal;
                            enFaseDeteccion = false;
                        }
                    }
                    else
                    {
                        rend.material = materialNormal;
                        tiempoMatarActual = tiempoNecesarioParaMatar;
                    }
                }
                else
                {
                    enFaseDeteccion = false;
                }

                yield return null;
            }

            // SALIDA
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

            // SPAWN
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
