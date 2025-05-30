using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionForwarder : MonoBehaviour
{
    public EliminatePlayer parentScript;

    private void OnCollisionEnter(Collision collision)
    {
        parentScript?.OnChildCollision(collision);
    }
}
