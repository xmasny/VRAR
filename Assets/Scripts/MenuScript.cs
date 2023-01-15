using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGameMulti()
    {
        PlayerPrefs.SetInt("CONTINUE", 0);
        PlayerPrefs.SetInt("SINGLE", 0);
        SceneManager.LoadScene(1);
    }

    public void PlayGameSingle()
    {
        PlayerPrefs.SetInt("CONTINUE", 0);
        PlayerPrefs.SetInt("SINGLE", 1);
        SceneManager.LoadScene(1);
    }


    public void ContinueGame()
    {
        PlayerPrefs.SetInt("CONTINUE", 1);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
