using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleToungue : MonoBehaviour
{
    public Camera camera;
    public GameObject toungue;
    public float displaceX, displaceY, displaceZ;

    void Update()
    {
        Vector3 direccion = camera.transform.eulerAngles;
        direccion.x += displaceX;
        direccion.y += displaceY;
        direccion.z += displaceZ;
        toungue.transform.rotation = Quaternion.Euler(direccion);
    }
}
