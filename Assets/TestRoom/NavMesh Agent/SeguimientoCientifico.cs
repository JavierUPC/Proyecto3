using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CientificoPerseguidor : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject camaleon;

    [Header("Puntos (coordenadas manuales XZ)")]
    public Vector3 spawnPosition;
    public Vector3 exitPosition;

    [Header("Configuración de tiempos y distancias")]
    public float tiempoEsperaEntreCiclos = 2f;
    public float tiempoDeteccion = 3f;
    public float distanciaDeteccion = 5f;

    [Header("Colores")]
    public Color colorNormal = Color.white;
    public Color colorDeteccion = Color.red;

    private NavMeshAgent agent;
    private Renderer rend;
    private bool enDeteccion = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();
        rend.material.color = colorNormal;

        StartCoroutine(CicloBusqueda());
    }

    private IEnumerator CicloBusqueda()
    {
        while (true)
        {
            // Buscar al camaleón hasta detectarlo
            while (Vector3.Distance(transform.position, camaleon.transform.position) > distanciaDeteccion)
            {
                agent.SetDestination(camaleon.transform.position);
                yield return null;
            }

            // Detección activa
            enDeteccion = true;
            rend.material.color = colorDeteccion;
            float tiempoRestante = tiempoDeteccion;

            while (tiempoRestante > 0f)
            {
                agent.SetDestination(camaleon.transform.position);
                tiempoRestante -= Time.deltaTime;
                yield return null;
            }

            // Termina la detección
            enDeteccion = false;
            rend.material.color = colorNormal;

            // Ir al punto de salida (solo XZ)
            agent.SetDestination(FijarAltura(exitPosition));
            yield return new WaitUntil(() => HaLlegadoDestino(exitPosition));

            // Ir al punto de spawn (solo XZ)
            agent.SetDestination(FijarAltura(spawnPosition));
            yield return new WaitUntil(() => HaLlegadoDestino(spawnPosition));

            // Esperar el tiempo de descanso
            yield return new WaitForSeconds(tiempoEsperaEntreCiclos);
        }
    }

    private Vector3 FijarAltura(Vector3 destino)
    {
        // Mantiene la Y actual del científico
        return new Vector3(destino.x, transform.position.y, destino.z);
    }

    private bool HaLlegadoDestino(Vector3 destino)
    {
        float distanciaXZ = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(destino.x, destino.z)
        );

        return distanciaXZ <= 1f; // Puedes ajustar este margen (por ejemplo 0.5 metros)
    }



}
