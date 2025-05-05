using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lengua : MonoBehaviour
{
    public ApplyAbilty player;
    public Camera cameraRef; 
    public bool firing = false; 
    public float rayDistance = 100f; 

    void Update()
    {
        if (!firing || cameraRef == null) return;

        Ray ray = new Ray(cameraRef.transform.position, cameraRef.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Mosca"))
            {
                //Debug.Log("Mosca hit by ray");
                GameObject mosca = hit.collider.gameObject;

                player.GetComponent<ApplyAbilty>().Abilty(mosca.GetComponentInParent<FlyType>().moscaSO.mosca);

                Destroy(mosca.GetComponentInParent<MovimientoMosca>().centroRotacion.gameObject);
                Destroy(mosca.transform.parent.gameObject);
                Destroy(mosca.gameObject);
            }
        }
    }
}
