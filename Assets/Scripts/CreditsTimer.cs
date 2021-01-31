using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsTimer : MonoBehaviour
{
    void Start() {
        StartCoroutine(CloseGame());
    }

    private IEnumerator CloseGame()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
