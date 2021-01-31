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

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hurtSound;
    
    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        currentWave = 1;
        enemiesKilled = 0;
        UpdateWave();
    }

    public void AddEnemyKillCount() {
        audioSource.PlayOneShot(hurtSound, 1f);
        enemiesKilled++;
        if(enemiesKilled >= waveThreeEnemyCount + waveTwoEnemyCount + waveOneEnemyCount) {
            UIManager.Instance.ShowWinPanel();
        } else if(enemiesKilled >= waveTwoEnemyCount + waveOneEnemyCount) {
            if(currentWave != 3) {
                UIManager.Instance.StartWaveCooldown();
            }
            currentWave = 3;
        } else if(enemiesKilled >= waveOneEnemyCount) {
            if(currentWave != 2) {
                UIManager.Instance.StartWaveCooldown();
            }
            currentWave = 2;
        }
    }

    public void UpdateWave() {
        UIManager.Instance.UpdateWaveText(currentWave);

        for(int i = 0; i < currentWave*2; i++) {
            spawns[Random.Range(0, spawns.Length)].StartNextWave();
        }
    }
}
