using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
    }

    //start new game
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    //pause game - call on pause input
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    //resume game
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    //return to menu
    public void ReturnGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    //quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
