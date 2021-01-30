using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float spawnFrequency = 5f;
    void Start()
    {
        StartCoroutine (SpawnEnemyTimer ());
    }

    public void SpawnEnemy() {
        GameObject itemObject = Instantiate (enemyPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator SpawnEnemyTimer () {
        yield return new WaitForSeconds (spawnFrequency);
        SpawnEnemy();
        StartCoroutine (SpawnEnemyTimer ());
    }
}
