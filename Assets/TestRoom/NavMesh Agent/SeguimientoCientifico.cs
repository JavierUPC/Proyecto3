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

    [Header("Audio pasos")]
    public AudioSource audioSourcePasos;
    public AudioClip pasoIzquierdo;
    public AudioClip pasoDerecho;

    [Tooltip("Factor de ajuste del ritmo de pasos. A mayor valor, más rápidos los pasos.")]
    [Range(0.1f, 5f)]
    public float frecuenciaPasos = 1.0f;

    [Tooltip("Tiempo mínimo absoluto entre pasos para evitar solapamientos.")]
    public float tiempoMinimoEntrePasos = 0.2f;

    private float tiempoUltimoPaso = 0f;
    private bool pieDerecho = false;




    private bool pasosActivos = false;
    private bool pasoDerecha = true; // Alternancia

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

        //ANDAR
        if (estaCaminando)
        {
            float velocidad = agent.velocity.magnitude;
            float delay = Mathf.Max(tiempoMinimoEntrePasos, 1f / (velocidad * frecuenciaPasos));

            if (Time.time - tiempoUltimoPaso >= delay)
            {
                AudioClip clip = pieDerecho ? pasoDerecho : pasoIzquierdo;
                audioSourcePasos.PlayOneShot(clip);
                pieDerecho = !pieDerecho;
                tiempoUltimoPaso = Time.time;
            }
        }

    }

    private IEnumerator ReproducirPasos()
    {
        bool pasoDerecha = true;

        while (true)
        {
            if (agent.velocity.magnitude > umbralVelocidad)
            {
                AudioClip clip = pasoDerecha ? pasoDerecho : pasoIzquierdo;
                audioSourcePasos.PlayOneShot(clip);
                pasoDerecha = !pasoDerecha;

                // Calcula el tiempo entre pasos en función de la velocidad
                float delay = 1.0f / (agent.velocity.magnitude * frecuenciaPasos);
                delay = Mathf.Max(delay, tiempoMinimoEntrePasos); // asegura que no se solapen
                yield return new WaitForSeconds(delay);
            }
            else
            {
                yield return null;
            }
        }
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

            //Asegura que empieza siempre con el material normal al salir de spawn
            rend.material = materialNormal;

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

            //Una vez termina la detección, vuelve al material normal por si quedó en detección pero no lo mató
            rend.material = materialNormal;

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
