using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lengua : MonoBehaviour
{
    public ApplyAbilty player;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisiona");
        if (collision.transform.tag == "Mosca")
        {
            Debug.Log("Entra aquí");
            player.GetComponent<ApplyAbilty>().Abilty(collision.gameObject.GetComponent<FlyType>().moscaSO.mosca);
            Destroy(collision.gameObject.GetComponent<MovimientoMosca>().centroRotacion.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
