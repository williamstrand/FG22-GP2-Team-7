using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] Animator animator;
    private int sceneToLoad;

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

    public void FadeToScene(int levelIndex)
    {
        sceneToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    //start new game
    public void PlayGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    //pause game - call on pause input
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    //resume game
    public void ResumeGame()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    //return to menu
    public void ReturnGame(int levelIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }

    //quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
