using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
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
        if(enemiesKilled >= waveOneEnemyCount) {
            currentWave = 2;
        } else if(enemiesKilled >= waveTwoEnemyCount + waveOneEnemyCount) {
            currentWave = 3;
        } else if(enemiesKilled >= waveThreeEnemyCount + waveTwoEnemyCount + waveOneEnemyCount) {
            UIManager.Instance.ShowWinPanel();
        }
    }
}
