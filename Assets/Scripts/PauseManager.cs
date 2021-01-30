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

    public void Awake () {
        Instance = this;
        panel.SetActive(false);
    }

    public void OnPause() {
        Time.timeScale = 0;
        panel.SetActive(true);
    }

    public void OnResumePress() {
        Time.timeScale = 1f;
        panel.SetActive(false);
    }

    public void OnRestartPress() {
        Time.timeScale = 1f;
        if(UIManager.Instance.isLevelTutorial()) {
            SceneManager.LoadScene(0);
        } else {
            SceneManager.LoadScene(1);
        }
    }

    public void OnExitPress() {
        Time.timeScale = 1f;
        if(UIManager.Instance.isLevelTutorial()) {
            Application.Quit();
        } else {
            SceneManager.LoadScene(0);
        }
    }
}
