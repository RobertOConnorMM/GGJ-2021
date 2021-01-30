using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
  public int countdown = 60;
  public int waveCooldown = 10;
  private int currWaveCooldown = 10;
  public TextMeshProUGUI timerText;
  public TextMeshProUGUI instructionText;
  public TextMeshProUGUI waveText;
  public TextMeshProUGUI waveCooldownText;

  [SerializeField]
  private GameObject winPanel, wavePanel, fadePanel;

  [SerializeField]
  private bool isTutorial;
  public bool hasFlashlight;
  public static UIManager Instance { get; private set; }

  public void Awake()
  {
    Instance = this;
    if (!isTutorial)
    {
      UpdateTimerText();
    }
  }

  void Start()
  {
    winPanel?.SetActive(false);
    fadePanel?.SetActive(true);
    waveText.text = "";
    waveCooldownText.text = "";

    if (!isTutorial)
    {
      StartCoroutine(UpdateCountdown());
      waveText.text = "Wave 1";
    }
    currWaveCooldown = waveCooldown;
  }

  public void ShowWinPanel()
  {
    Time.timeScale = 0;
    winPanel.SetActive(true);
  }

  public void UpdateWaveText(int waveNum) {
      waveText.text = "Wave " + waveNum;
  }

  private void UpdateTimerText()
  {
    var timeSpan = TimeSpan.FromSeconds(countdown);
    timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
  }

  private IEnumerator UpdateCountdown()
  {
    yield return new WaitForSeconds(1f);
    countdown -= 1;

    UpdateTimerText();
    if (countdown > 0)
    {
      StartCoroutine(UpdateCountdown());
    }
    else
    {
      ShowWinPanel();
    }
  }

    public void StartWaveCooldown() {
        currWaveCooldown = waveCooldown;
        StartCoroutine(UpdateWaveCooldown());
    }

  private IEnumerator UpdateWaveCooldown()
  {
    yield return new WaitForSeconds(1f);
    currWaveCooldown -= 1;
    waveCooldownText.text = "Next wave in... "+currWaveCooldown+"s" ;
    if(currWaveCooldown > 0) {
        StartCoroutine(UpdateWaveCooldown());
    } else {
        waveCooldownText.text = "";
        WaveManager.Instance.UpdateWave();
    }
  }

  private IEnumerator LoadMainScene()
  {
    yield return new WaitForSeconds(3f);
    SceneManager.LoadScene(1);
  }

  public bool isLevelTutorial()
  {
    return isTutorial;
  }

  public void StartGame()
  {
    if (Instance.hasFlashlight)
    {
      wavePanel.SetActive(true);
      isTutorial = false;
      StartCoroutine(LoadMainScene());
    }
    else
    {
      instructionText.text = "Grab your flashlight first!";
    }
  }

  public void OnCollectFlashLight()
  {
    hasFlashlight = true;
    instructionText.text = "Ok, now you're ready!";
  }
}
