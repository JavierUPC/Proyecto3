using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireAbilty : MonoBehaviour
{
    public bool fireAbility;
    public float rayDistance = 100f;
    public Camera cameraRef;
    public ApplyAbilty abilty;

    void Start()
    {
        
    }

    private void Update()
    {
        if (!fireAbility) return;

        //VISUALES

        Ray ray = new Ray(cameraRef.transform.position, cameraRef.transform.forward);
        RaycastHit hit;

        //Debug.Log("Firing Ability");
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Interact"))
            {
                hit.collider.transform.GetComponent<Button>().Activate(abilty.type);
            }
        }
    }
}
