using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel, wavePanel, fadePanel;
    [SerializeField]
    private TextMeshProUGUI timerText, instructionText;
    private int countdown = 30;

    [SerializeField]
    private bool isTutorial = false;
    public bool hasFlashlight = false;
    public static UIManager Instance { get; private set; }

    public void Awake () {
        Instance = this;
        if(!isTutorial) {
            UpdateTimerText();
        }
    }

    void Start()
    {
        winPanel.SetActive(false);
        fadePanel.SetActive(true);
        if(!isTutorial) {
            StartCoroutine(UpdateCountdown());
        }
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

    private IEnumerator LoadMainScene() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

    public bool isLevelTutorial() {
        return isTutorial;
    }

    public void StartGame() {
        if(UIManager.Instance.hasFlashlight) {
            wavePanel.SetActive(true);
            isTutorial = false;
            StartCoroutine(LoadMainScene());
        } else {
            instructionText.text = "Grab your flashlight first!";
        }
    }

    public void OnColletFlashLight() {
        hasFlashlight = true;
        instructionText.text = "Ok, now you're ready!";
    }
}
