using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMosca : MonoBehaviour
{
    public SpawnerMosca spawnerMosca;

    public float velocidad = 2f;
    public float escala = 3f;
    public float velocidadRotacion = 30f;
    public float tiempoHuyendo = 2f;
    public float amplitudSerpenteo = 1f;
    public float frecuenciaSerpenteo = 5f;

    private Rigidbody rb;
    private float tiempo = 0f;
    public Transform centroRotacion;
    private Vector3 ultimaPosicion;
    private bool fleeing;
    private float timer;
    private Vector3 direccionOpuesta;
    private Vector3 direccionPerpendicular;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        centroRotacion = new GameObject("CentroOcho").transform;
        centroRotacion.position = transform.position;

        ultimaPosicion = transform.position;

        fleeing = false;
    }

    void FixedUpdate()
    {
        if (!fleeing)
        {
            timer = 0;
            tiempo += Time.fixedDeltaTime * velocidad;

            float x = escala * Mathf.Sin(tiempo);
            float y = escala * Mathf.Sin(tiempo) * Mathf.Cos(tiempo);
            float z = 0f;

            centroRotacion.Rotate(Vector3.up * velocidadRotacion * Time.fixedDeltaTime);
            Vector3 nuevaPosicion = centroRotacion.TransformPoint(new Vector3(x, y, z));

            Vector3 velocidadDeseada = (nuevaPosicion - rb.position) / Time.fixedDeltaTime;
            rb.velocity = velocidadDeseada;

            Vector3 direccionMovimiento = nuevaPosicion - ultimaPosicion;
            if (direccionMovimiento != Vector3.zero)
            {
                Quaternion rotacionDeseada = Quaternion.LookRotation(direccionMovimiento);
                rb.MoveRotation(rotacionDeseada);
            }

            ultimaPosicion = nuevaPosicion;
        }
        else
        {
            timer += Time.deltaTime;

            float oscilacion = Mathf.Sin(Time.time * frecuenciaSerpenteo) * amplitudSerpenteo;
            Vector3 serpentineMovement = direccionOpuesta + (direccionPerpendicular * oscilacion);

            rb.velocity = serpentineMovement * velocidad;

            centroRotacion.position = rb.position;

            if (timer >= tiempoHuyendo)
            {
                fleeing = false;
                tiempo = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 normalContacto = (transform.position - other.transform.position).normalized;
            direccionOpuesta = normalContacto;
            direccionPerpendicular = Vector3.Cross(direccionOpuesta, Vector3.up).normalized;

            //Debug.Log($"Normal: {normalContacto}, Dirección Opuesta: {direccionOpuesta}, Perpendicular: {direccionPerpendicular}");

            StartCoroutine(FleeingActive());
        }
    }

    private IEnumerator FleeingActive()
    {
        yield return new WaitForSeconds(0.1f);
        fleeing = true;
        timer = 0;
    }

    public void ActivateSpawner()
    {
        spawnerMosca.SetSpawn();
    }
}
