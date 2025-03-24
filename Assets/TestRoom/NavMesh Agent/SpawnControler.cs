using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject enemy; // Enemigo
    public GameObject character; // Personaje

    public Vector3 enemySpawnPosition; // Coordenadas del enemigo
    public Vector3 characterStartPosition; // Coordenadas del personaje

    public float spawnInterval = 5f; // Tiempo antes de que el enemigo aparezca
    public float waitTimeAtTarget = 3f; // Tiempo que esperan antes de desaparecer

    private bool isFollowing = false;

    void Start()
    {
        StartCoroutine(EnemyCycleRoutine());
    }

    IEnumerator EnemyCycleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Aparece el enemigo en la posición especificada
            enemy.transform.position = enemySpawnPosition;
            enemy.SetActive(true);

            // Coloca el personaje en su posición inicial y lo activa
            character.transform.position = characterStartPosition;
            character.SetActive(true);
            isFollowing = true;

            // Esperar a que el personaje llegue al enemigo
            while (isFollowing)
            {
                float distance = Vector3.Distance(character.transform.position, enemy.transform.position);
                if (distance <= 1.0f) // Ajusta este valor según el radio de detección
                {
                    isFollowing = false;
                    yield return new WaitForSeconds(waitTimeAtTarget);

                    // Desaparecen ambos
                    enemy.SetActive(false);
                    character.SetActive(false);
                }

                yield return null;
            }
        }
    }
}
