using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoMosca : MonoBehaviour
{
    public float velocidad = 2f; 
    public float escala = 3f; 
    public float velocidadRotacion = 30f; 

    private Rigidbody rb;
    private float tiempo = 0f;
    private Transform centroRotacion;
    private Vector3 ultimaPosicion;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        centroRotacion = new GameObject("CentroOcho").transform;
        centroRotacion.position = transform.position;

        ultimaPosicion = transform.position;
    }

    void FixedUpdate()
    {
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
}
