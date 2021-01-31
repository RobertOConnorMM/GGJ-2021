using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
  [SerializeField]
  private GameObject enemyPrefab;
  private float spawnFrequency = 10f;
  public int spawnLimit = 1;
  public int spawnCount = 0;
  public bool hasStarted = false;

  public void SpawnEnemy()
  {
    GameObject itemObject = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
  }

  public void StartNextWave() {
    if(!hasStarted) {
      spawnLimit = 1;
      spawnCount = 0;
      StartCoroutine(SpawnEnemyTimer(3f));
      hasStarted = true;
    } else {
      spawnLimit++;
    }
  }

  private IEnumerator SpawnEnemyTimer(float freq)
  {
    yield return new WaitForSeconds(freq);
    SpawnEnemy();
    spawnCount++;
    if (spawnCount < spawnLimit)
    {
      StartCoroutine(SpawnEnemyTimer(spawnFrequency));
    } else {
      hasStarted = false;
    }
  }
}
