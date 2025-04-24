using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireAbilty : MonoBehaviour
{
    public bool fireAbility;
    public float rayDistance = 100f;
    public Camera cameraRef;

    void Start()
    {
        
    }

    private void Update()
    {
        if (!fireAbility|| cameraRef == null) return;

        Ray ray = new Ray(cameraRef.transform.position, cameraRef.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Interact"))
            {
                
            }
        }
    }
}
