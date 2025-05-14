using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMosca : MonoBehaviour
{
    public GameObject mosca;
    private bool spawn = false;
    public int spawnTimer;

    private void Start()
    {
        GameObject spawnedFly = Instantiate(mosca, transform.position, transform.rotation);
        spawnedFly.GetComponent<MovimientoMosca>().spawnerMosca = this;
    }

    private void FixedUpdate()
    {
        if (spawn)
        {
            GameObject spawnedFly = Instantiate(mosca, transform.position, transform.rotation);
            spawnedFly.GetComponent<MovimientoMosca>().spawnerMosca = this;
            spawn = false;
        }

    }

    public void SetSpawn()
    {
        StartCoroutine(SetTimer());
    }

    private IEnumerator SetTimer()
    {
        yield return new WaitForSeconds(spawnTimer);
        spawn = true;
    }
}
