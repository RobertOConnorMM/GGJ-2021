using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
  public int currentWave = 1;
  public int countdown = 30;
  public TextMeshProUGUI timerText;
  public TextMeshProUGUI instructionText;

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

    if (!isTutorial)
    {
      StartCoroutine(UpdateCountdown());
    }
  }

  public void ShowWinPanel()
  {
    Time.timeScale = 0;
    winPanel.SetActive(true);
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
