using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    public bool On;

    private void Start()
    {
        On = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("DEBERÍA ENTRAR EN KILL: " + collision.transform.tag + "and " + On);
        if (On && collision.transform.CompareTag("Player"))
        {
            Kill.Reload();
        }
    }
}
