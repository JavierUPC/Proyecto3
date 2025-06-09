using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DeadBy
{
    Cactus,
    Cable
}

public class EliminatePlayer : MonoBehaviour
{
    public bool On;
    public CallScreen callScreen;
    public DeadBy deadBy;

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
            if (deadBy == DeadBy.Cable)
            {
                callScreen.Electricity();
            }
            else if(deadBy == DeadBy.Cactus)
            {
                callScreen.Cactus();
            }
        }
    }
}
