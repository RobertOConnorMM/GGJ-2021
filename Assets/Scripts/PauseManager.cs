using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField]
    private GameObject panel;
    public bool isPaused = false;

    public void Awake () {
        Instance = this;
        panel.SetActive(false);
    }

    public void OnPause() {
        if(!isPaused) {
            Time.timeScale = 0;
            panel.SetActive(true);
            isPaused = true;
        } else {
            OnResumePress();
        }
    }

    public void OnResumePress() {
        Time.timeScale = 1f;
        panel.SetActive(false);
        isPaused = false;
    }

    public void OnRestartPress() {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(0);
    }

    public void OnExitPress() {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(2);
    }
}
