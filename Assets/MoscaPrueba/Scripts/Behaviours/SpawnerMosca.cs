using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMosca : MonoBehaviour
{
    public GameObject mosca;
    private bool spawn = true;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (spawn)
        {
            //Instanciar mosca
            spawn = false;
        }

    }

    public void SetSpawn()
    {
        spawn = true;
    }
}
