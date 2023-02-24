using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIAudioManager : MonoBehaviour
{
    AudioSource[] audioSources;
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip pressedSound;
    int currentScene;

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(currentScene);
    }

    private void Start()
    {
        if (currentScene != 1)
        {
            audioSources[1].Play();
            audioSources[1].loop = true;
        }
        
    }

    public void hoverButton()
    {
        audioSources[0].PlayOneShot(hoverSound);
    }
    public void clickButton()
    {
        audioSources[0].PlayOneShot(pressedSound);
    }
}
