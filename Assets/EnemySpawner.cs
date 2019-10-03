using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float delay;
    public Enemy[] enemies;
    int index = 0;
    bool canSpawn = false;

    void Start()
    {
        StartCoroutine(SpawnDelay(delay));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemies.Length > 0 && canSpawn)
        {
            if(index < enemies.Length)
            {
                Instantiate(enemies[index].gameObject, transform.position, Quaternion.identity);
                canSpawn = false;
                StartCoroutine(SpawnDelay(delay));
            }
            else
            {
                index = 0;
                canSpawn = true;
            }
        }
    }

    IEnumerator SpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSpawn = true;
    }
}
