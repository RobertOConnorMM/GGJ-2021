using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    public EnemySpawn[] spawns;
    public int currentWave = 1;
    private int waveOneEnemyCount = 2;
    private int waveTwoEnemyCount = 4;
    private int waveThreeEnemyCount = 6;
    public int enemiesKilled = 0;

    
    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentWave = 1;
        enemiesKilled = 0;
    }

    public void AddEnemyKillCount() {
        enemiesKilled++;
        if(enemiesKilled >= waveThreeEnemyCount + waveTwoEnemyCount + waveOneEnemyCount) {
            UIManager.Instance.ShowWinPanel();
        } else if(enemiesKilled >= waveTwoEnemyCount + waveOneEnemyCount) {
            if(currentWave != 3) {
                UpdateWave(3);
            }
            currentWave = 3;
        } else if(enemiesKilled >= waveOneEnemyCount) {
            if(currentWave != 2) {
                UpdateWave(2);
            }
            currentWave = 2;
        }
    }

    private void UpdateWave(int waveNum) {
        currentWave = waveNum;
        UIManager.Instance.UpdateWaveText(waveNum);
        for(int i = 0; i < spawns.Length; i++) {
            spawns[i].StartNextWave(waveNum);
        }
    }
}
