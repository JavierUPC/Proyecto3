using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminatePlayer : MonoBehaviour
{
    public bool On;

    private void Start()
    {
        On = true;

        // Find all colliders in children and add CollisionForwarder
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in childColliders)
        {
            CollisionForwarder forwarder = col.gameObject.GetComponent<CollisionForwarder>();
            if (forwarder == null)
            {
                forwarder = col.gameObject.AddComponent<CollisionForwarder>();
            }
            forwarder.parentScript = this;
        }
    }

    // This method gets called by CollisionForwarder on any child collider
    public void OnChildCollision(Collision collision)
    {
        if (On && collision.transform.CompareTag("Player"))
        {
            Kill.Reload();
        }
    }
}
