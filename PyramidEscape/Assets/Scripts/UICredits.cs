using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UICredits : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }
    public void GoToMainMenuScene()
    {
        StartCoroutine("LoadMainMenuScene");
    }

    public void PauseCreditsRoll()
    {
        Time.timeScale = 0;
    }

    public void ResumeCreditsRoll()
    {
        Time.timeScale = 1;
    }

    IEnumerator LoadMainMenuScene()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Main Menu");
    }
}
