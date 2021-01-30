using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
  [SerializeField]
  private GameObject enemyPrefab;
  [SerializeField]
  private float spawnFrequency = 5f;
  public int spawnLimit = 1;
  public int spawnCount = 0;
  void Start()
  {
    StartCoroutine(SpawnEnemyTimer());
  }

  public void SpawnEnemy()
  {
    GameObject itemObject = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
  }

  public void StartNextWave(int waveNum) {
    spawnLimit = waveNum;
    spawnCount = 0;
    StartCoroutine(SpawnEnemyTimer());
  }

  private IEnumerator SpawnEnemyTimer()
  {
    yield return new WaitForSeconds(spawnFrequency);
    SpawnEnemy();
    spawnCount++;
    if (spawnCount < spawnLimit)
    {
      StartCoroutine(SpawnEnemyTimer());
    }
  }
}
