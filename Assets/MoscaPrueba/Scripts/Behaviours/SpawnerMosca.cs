using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMosca : MonoBehaviour
{
    public MoscaSO moscaSO;
    private bool spawn = true;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (spawn)
        {
            if (moscaSO.mosca == TipoMosca.Electrico)
            {
                //InstanciarMosca eléctrica
                spawn = false;
            }
            if (moscaSO.mosca == TipoMosca.Fuego)
            {
                //InstanciarMosca de fuego
                spawn = false;
            }
        }

    }

    public void SetSpawn()
    {
        spawn = true;
    }
}
