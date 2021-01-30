using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private TextMeshProUGUI timerText;
    private int countdown = 10;
    public static UIManager Instance { get; private set; }

    public void Awake () {
        Instance = this;
        UpdateTimerText();
    }

    void Start()
    {
        winPanel.SetActive(false);
        StartCoroutine(UpdateCountdown());
    }

    public void ShowWinPanel() {
        Time.timeScale = 0;
        winPanel.SetActive(true);
    }

    private void UpdateTimerText() {
        timerText.text = "00:" + countdown.ToString("d2");
    }

    private IEnumerator UpdateCountdown() {
        yield return new WaitForSeconds(1f);
        countdown -= 1;
        UpdateTimerText();
        if(countdown > 0) {
            StartCoroutine(UpdateCountdown());
        } else {
            ShowWinPanel();
        }
  }
}
