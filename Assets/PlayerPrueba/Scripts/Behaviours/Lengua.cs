using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lengua : MonoBehaviour
{
    public ApplyAbilty player;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mosca")
        {
            Debug.Log("Entra aquí");
            player.GetComponent<ApplyAbilty>().Abilty(collision.gameObject.GetComponent<FlyType>().moscaSO.mosca);
            Destroy(collision.gameObject.transform.parent.gameObject);

        }
    }
}
