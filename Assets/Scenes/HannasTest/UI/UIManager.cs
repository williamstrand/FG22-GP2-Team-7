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

    //fade to a scene
    public void FadeToScene(int levelIndex)
    {
        sceneToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void FadeComplete()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }


    //pause game - call on pause input
    public void PauseGame()
    {
        pauseCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    //resume game
    public void ResumeGame()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    //quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
